using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Owls.DTOs;
using Owls.Models;
using Owls.Repositories.ColorRepos;
using Owls.Repositories.ProductRepos;
using Owls.Services.Firebase;

namespace Owls.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepos productRepos;
        private readonly IColorRepos colorRepos;

        private readonly IFirebaseStorage firebaseStorage;
        private readonly IMapper mapper;

        public ProductsController(IProductRepos productRepos, IFirebaseStorage firebase, IMapper mp, IColorRepos color)
        {
            this.productRepos = productRepos;
            this.firebaseStorage = firebase;
            this.mapper = mp;
            this.colorRepos = color;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Route("Category/{query?}/{page:int?}/{sortOrder?}/{priceRange?}/{colorId?}")]

        public async Task<IActionResult> ProductsFilterByCategory(string query, int? page, Filter model)
        {
            ViewBag.Func = "ProductsFilterByCategory";
            ViewBag.Query = query;
            IEnumerable<Product> products = await productRepos.FilterProducts(query, null, model.PriceRange, model.ColorId);
            return await ProcessFilteredProducts(products, page, model);
        }
        [Route("Search/{query?}/{page:int?}/{sortOrder?}/{priceRange?}/{colorId?}")]

        public async Task<IActionResult> ProductsFilterByName(string query, int? page, Filter model)
        {
            IEnumerable<Product> products = await productRepos.FilterProducts(null, query, model.PriceRange, model.ColorId);
            ViewBag.Func = "ProductsFilterByName";
            ViewBag.Query = query;
            return await ProcessFilteredProducts(products, page, model);
        }

        private async Task<IActionResult> ProcessFilteredProducts(IEnumerable<Product> products, int? page, Filter filter)
        {


            var productReadVM = mapper.Map<IEnumerable<ProductBaseInformation>>(products);

            if (!string.IsNullOrEmpty(filter.SortOrder))
            {
                if (filter.SortOrder.Trim().Equals("price_asc"))
                {
                    productReadVM = productReadVM.OrderBy(x => x.Price).ToList();
                }
                else
                {
                    productReadVM = productReadVM.OrderByDescending(x => x.Price).ToList();
                }
            }

            if (page == null || page < 1) { page = 1; }
            Pager pager = new Pager() { ItemsPerPage = 12, CurrentPage = (int)page, TotalItems = productReadVM.Count() };
            productReadVM = productReadVM.Skip(pager.ItemsPerPage * ((int)page - 1)).Take(pager.ItemsPerPage);

            ViewBag.Pager = pager;
            foreach (var item in productReadVM)
            {
                if (item.Imagethumbnail != null)
                {
                    item.Imagethumbnail = await firebaseStorage.GetImageStreamAsync(item.Imagethumbnail);
                }
            }



            var colors = await colorRepos.GetColors();
            ViewBag.Colors = colors;

            ProductListVM rs = new ProductListVM()
            {
                Filter = filter,
                Products = productReadVM,
                PagingInfo = pager
            };


            return View("Index", rs);
        }

        [Route("Products/{proName}")]
        public async Task<IActionResult> ProductDetails(string proName)
        {
            var products = await productRepos.GetProductByName(proName);
            var product = products.FirstOrDefault();
            if (product == null) { return RedirectToAction("Index", "Home"); }
            var productReadVM = mapper.Map<ProductReadVM>(product);

            productReadVM.Imagethumbnail = await firebaseStorage.GetImageStreamAsync(productReadVM.Imagethumbnail);
            for (int i = 0; i < productReadVM.Images.Count; i++)
            {
                if (productReadVM.Images[i] != null)
                {
                    productReadVM.Images[i] = await firebaseStorage.GetImageStreamAsync(productReadVM.Images[i].ToString());
                }
            }
            return View("Details", productReadVM);
        }


    }
}
