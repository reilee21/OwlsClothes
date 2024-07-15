using Owls.DTOs;
using Owls.Models;

namespace Owls.Repositories.ProductRepos
{
	public interface IProductRepos
	{
		Task<Product> GetProduct(int id);
		Task<CartItem> GetProductCart(string sku, int quantity);

		Task<Product> GetProductByName(string name);

		Task<IEnumerable<Product>> GetNewestProducts(int count);
		Task<IEnumerable<Product>> GetBestSellsProducts(int count);

		Task<IEnumerable<Product>> FilterProducts(string category);
		Task<IEnumerable<Product>> FilterProducts(string category, string searchString, string price_range, string colorId);

	}

	public interface IProductVariant
	{
		Task<ProductVariant> GetVariant(string sku);

	}

}
