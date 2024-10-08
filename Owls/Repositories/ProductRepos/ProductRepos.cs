﻿using Microsoft.EntityFrameworkCore;
using Owls.DTOs;
using Owls.Models;
using System.Web;

namespace Owls.Repositories.ProductRepos
{
	public class ProductRepos : IProductRepos
	{
		private readonly OwlStoreContext _storeContext;

		public ProductRepos(OwlStoreContext storeContext)
		{
			_storeContext = storeContext;
		}


		public async Task<IEnumerable<Product>> FilterProducts(string category_tag)
		{
			var productsInCategory = await _storeContext.Products.Include(p => p.ProductVariants).Where(p => p.Category.Tag == category_tag && p.IsActive == true).ToListAsync();
			if (productsInCategory.Any())
			{ return productsInCategory; }
			return new List<Product>();
		}
		public async Task<IEnumerable<Product>> FilterProducts(string category, string searchString, string? price_range, string? colorId)
		{
			IEnumerable<Product> query = new List<Product>();
			if (string.IsNullOrEmpty(searchString))
			{
				bool isParentCate = await IsParentCate(category);
				if (isParentCate)
				{
					Category c = await _storeContext.Categories.FirstOrDefaultAsync(c => c.Tag.Equals(category));
					query = await _storeContext.Products.Include(p => p.ProductVariants).Where(p => p.Category.ParentCate == c.CategoryId && p.IsActive == true).ToListAsync();

				}
				else
				{
					query = await _storeContext.Products.Include(p => p.ProductVariants).Where(p => p.Category.Tag == category && p.IsActive == true).ToListAsync();
				}
			}
			else
			{
				query = await _storeContext.Products.Include(p => p.ProductVariants).Where(p => p.Name.ToLower().Contains(searchString.ToLower()) && p.IsActive == true).ToListAsync();
			}

			if (!string.IsNullOrEmpty(price_range))
			{
				switch (price_range)
				{
					case "under_100k":
						query = query.Where(p => p.ProductVariants.Any(v => v.SalePrice < 100000));
						break;
					case "100k_200k":
						query = query.Where(p => p.ProductVariants.Any(v => v.SalePrice >= 100000 && v.SalePrice < 200000));
						break;
					case "200k_300k":
						query = query.Where(p => p.ProductVariants.Any(v => v.SalePrice >= 200000 && v.SalePrice < 300000));
						break;
					case "300k_500k":
						query = query.Where(p => p.ProductVariants.Any(v => v.SalePrice >= 300000 && v.SalePrice < 500000));
						break;
					case "over_500k":
						query = query.Where(p => p.ProductVariants.Any(v => v.SalePrice >= 500000));
						break;
				}
			}
			if (!string.IsNullOrEmpty(colorId))
			{
				int cl = int.Parse(colorId);
				query = query.Where(p => p.ProductVariants.Any(v => v.ColorId == cl));
			}
			foreach (var item in query)
			{
				foreach (var variant in item.ProductVariants)
				{
					variant.Promotions = _storeContext.Promotions
						.Where(pr => pr.Products.Any(pv => pv.Sku == variant.Sku) && pr.EndDate.Date >= DateTime.Today.Date)
						.ToList();
				}
			}

			return query;

		}
		public async Task<Product> GetProduct(int id)
		{
			var prod = await _storeContext.Products.Include(p => p.ProductVariants).Include(p => p.ProductImages).FirstOrDefaultAsync(p => p.ProductId == id && p.IsActive == true);
			if (prod != null)
				return prod;
			else
				return new Product();
		}

		public async Task<IEnumerable<Product>> GetNewestProducts(int count)
		{
			var newestProducts = await _storeContext.Products.Include(p => p.ProductVariants).Where(p => p.IsActive == true)
				.OrderByDescending(p => p.CreateAt)
			.Take(count)
				.ToListAsync();
			foreach (var item in newestProducts)
			{
				foreach (var variant in item.ProductVariants)
				{
					variant.Promotions = _storeContext.Promotions
						.Where(pr => pr.Products.Any(pv => pv.Sku == variant.Sku) && pr.EndDate.Date >= DateTime.Today.Date)
						.ToList();
				}
			}
			return newestProducts;
		}

		public async Task<IEnumerable<Product>> GetBestSellsProducts(int count)
		{
			var topProducts = (from d in _storeContext.OrderDetails
							   join v in _storeContext.ProductVariants on d.Sku equals v.Sku into joined
							   from subV in joined.DefaultIfEmpty()
							   group d by new { d.Sku, subV.ProductId } into grouped
							   orderby grouped.Sum(x => x.Quantity) descending
							   select new
							   {
								   ProductId = grouped.Key.ProductId
							   }).ToList();

			if (!topProducts.Any())
			{
				return await _storeContext.Products.Where(p => p.IsActive == true).Take(count).ToListAsync();
			}
			List<Product> rs = new List<Product>();
			foreach (var item in topProducts)
			{
				var p = await _storeContext
					.Products.Include(p => p.ProductVariants)
					.FirstOrDefaultAsync(p => p.ProductId.Equals(item.ProductId) && p.IsActive == true);
				if (p != null)
				{
					rs.Add(p);
					if (rs.Count == count)
						break;
				}

			}
			foreach (var item in rs)
			{
				foreach (var variant in item.ProductVariants)
				{
					variant.Promotions = _storeContext.Promotions
						.Where(pr => pr.Products.Any(pv => pv.Sku == variant.Sku) && pr.EndDate.Date >= DateTime.Today.Date)
						.ToList();
				}
			}

			return rs;
		}


		public async Task<Product> GetProductByName(string name)
		{
			string decodedName = HttpUtility.UrlDecode(name);

			var product = await _storeContext.Products
				.Include(p => p.ProductVariants).ThenInclude(v => v.Color)
				.Include(p => p.ProductImages)
				.FirstOrDefaultAsync(p => p.Name == decodedName && p.IsActive == true);


			foreach (var variant in product.ProductVariants)
			{
				variant.Promotions = _storeContext.Promotions
					.Where(pr => pr.Products.Any(pv => pv.Sku == variant.Sku) && pr.EndDate.Date >= DateTime.Today.Date)
					.ToList();
			}
			return product;
		}


		public async Task<CartItem> GetProductCart(string sku, int quantity)
		{
			var proVa = await _storeContext.ProductVariants.Include(p => p.Product).FirstOrDefaultAsync(p => p.Sku == sku);
			if (proVa == null)
				return null;
			return new CartItem()
			{
				Sku = proVa.Sku,
				ProductName = proVa.Product.Name,
				Color = proVa.ColorId,
				Image = proVa.Product.Imagethumbnail,
				Price = proVa.SalePrice,
				Size = proVa.Size,
				StockQuantity = proVa.Quantity,
				Quantity = quantity
			};
		}

		private async Task<bool> IsParentCate(string tag)
		{
			var rs = await _storeContext.Categories.FirstOrDefaultAsync(c => c.Tag.Equals(tag));
			return rs.ParentCate == null;
		}
	}
}
