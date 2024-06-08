using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Owls.DTOs;
using Owls.DTOs.Write;
using Owls.Models;
using Owls.Services.Firebase;

namespace Owls.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly OwlStoreContext _storeContext;
        private readonly IFirebaseStorage firebaseStorage;
        private readonly IMapper mapper;

        public ProductsController(OwlStoreContext storeContext, IFirebaseStorage firebaseStorage, IMapper mapper)
        {
            _storeContext = storeContext;
            this.firebaseStorage = firebaseStorage;
            this.mapper = mapper;
        }
        public async Task<IActionResult> Index(int? page, string? search, int? cate)
        {
            if (page == null) page = 1;
            ViewBag.Nav = "Products";
            IEnumerable<Product> rs = new List<Product>();
            if (!string.IsNullOrEmpty(search) && cate != null)
            {
                rs = await _storeContext.Products.Include(c => c.Category)
              .Where(p => (p.CategoryId.Equals(cate) || p.Category.ParentCate.Equals(cate)) && p.Name.ToLower().Contains(search.ToLower()))
              .ToListAsync();
            }
            else if (search != null)
            {
                rs = await _storeContext.Products.Include(c => c.Category)
                 .Where(p => p.Name.ToLower().Contains(search.ToLower()))
                 .ToListAsync();
            } else if (cate != null)
            {
                rs = await _storeContext.Products.Include(c => c.Category)
                 .Where(p => p.CategoryId.Equals(cate) || p.Category.ParentCate.Equals(cate))
                 .ToListAsync();
            }
            else {
                rs = await _storeContext.Products.Include(c => c.Category).ToListAsync();

            }
            ViewBag.Pager = new Pager() { CurrentPage = (int)page, ItemsPerPage = 10, TotalItems = rs.Count() };
            rs = rs.Skip(10 * ((int)page - 1)).Take(10);

            foreach (var item in rs)
            {
                if (item.Imagethumbnail != null)
                {
                    item.Imagethumbnail = await firebaseStorage.GetImageStreamAsync(item.Imagethumbnail);
                }
            }
            var categories = await _storeContext.Categories.ToListAsync();
            ViewBag.Categories = categories;
            ViewBag.SelectedCate = cate;
            ViewBag.SearchString = search;

            return View(rs);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var cate = await _storeContext.Categories.ToListAsync();
            var colors = await _storeContext.Colors.ToListAsync();

            ViewBag.Categories = cate;
            ViewBag.Colors = colors;
            ViewBag.Nav = "Products";
            return View(new ProductWrite());
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductWrite pro)
        {
            var cate = await _storeContext.Categories.ToListAsync();
            var colors = await _storeContext.Colors.ToListAsync();

            ViewBag.Categories = cate;
            ViewBag.Colors = colors;
            ViewBag.Nav = "Products";
            if (ModelState.IsValid)
            {
                return View(new ProductWrite());
            }
            return View(pro);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int productId)
        {

            Product pro = await _storeContext.Products
                                            .Include(p => p.ProductVariants)
                                            .Include(p => p.ProductImages)
                                            .FirstOrDefaultAsync(p => p.ProductId.Equals(productId));
            if (pro == null) return RedirectToAction("Index");
            var cate = await _storeContext.Categories.ToListAsync();
            var colors = await _storeContext.Colors.Select(c => new { c.ColorId, c.ColorName }).ToListAsync();
            ProductWrite proWrite = mapper.Map<ProductWrite>(pro);

            for (int i = 0; i < proWrite.ImagesDisplay.Count; i++)
            {
                string img = proWrite.ImagesDisplay[i];
                if (!string.IsNullOrEmpty(img))
                {
                    proWrite.ImagesDisplay[i] = await firebaseStorage.GetImageStreamAsync(img);
                }
            }
            ViewBag.Nav = "Products";
            ViewBag.Categories = cate;
            ViewBag.Colors = colors;

            return View(proWrite);

        }

        [HttpPost]
        public async Task<IActionResult> RemoveVariant(string? sku)
        {
            if(string.IsNullOrEmpty(sku) ) return NotFound();

            ProductVariant variant = await _storeContext.ProductVariants.FindAsync(sku);
            if (variant == null) return NotFound();
            try
            {
                _storeContext.ProductVariants.Remove(variant);
                await _storeContext.SaveChangesAsync();
            }catch (Exception ex)
            {
                return BadRequest();
            }
            return Ok();
        }

    }
}
