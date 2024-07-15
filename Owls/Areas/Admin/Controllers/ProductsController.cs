using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Owls.DTOs;
using Owls.DTOs.Write;
using Owls.Models;
using Owls.Services.Firebase;

namespace Owls.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "AdminOrStaff")]

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
            if (page == null)
                page = 1;
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
            }
            else if (cate != null)
            {
                rs = await _storeContext.Products.Include(c => c.Category)
                 .Where(p => p.CategoryId.Equals(cate) || p.Category.ParentCate.Equals(cate))
                 .ToListAsync();
            }
            else
            {
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
            if (pro.ImagesOrder.Any() && pro.ImagesOrder[0] != null)
            {
                pro.ImagesOrder = System.Text.Json.JsonSerializer.Deserialize<List<string>>(pro.ImagesOrder[0]);
            }

            if (ModelState.IsValid)
            {
                for (int i = 0; i < pro.Varriants.Count; i++)
                {
                    var v = pro.Varriants[i];
                    for (int j = i + 1; j < pro.Varriants.Count; j++)
                    {
                        if (v.Sku.Equals(pro.Varriants[j].Sku))
                        {
                            ViewBag.Errors = "Trùng mã SKU: " + v.Sku;
                            return View(pro);
                        }
                    }

                    if (await ExistSKU(v.Sku))
                    {
                        ViewBag.Errors = "Trùng mã SKU: " + v.Sku;
                        return View(pro);
                    }

                }

                Product product = mapper.Map<Product>(pro);
                product.CreateAt = DateTime.Now;

                try
                {
                    _storeContext.Products.Add(product);
                    await _storeContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return View(pro);
                }

                foreach (var v in pro.Varriants)
                {
                    ProductVariant pv = mapper.Map<ProductVariant>(v);
                    pv.ProductId = product.ProductId;
                    _storeContext.ProductVariants.AsNoTracking().Append(pv);
                }

                List<string> image = await UploadImage(pro.Images, pro.ImagesOrder, product.ProductId);
                foreach (string i in image)
                {
                    ProductImage pi = new ProductImage()
                    {
                        Name = i,
                        ProductId = product.ProductId
                    };
                    _storeContext.ProductImages.Add(pi);
                    await _storeContext.SaveChangesAsync();
                }
                product.Imagethumbnail = product.ProductId.ToString() + "_0.jpg";
                _storeContext.Entry(product).State = EntityState.Modified;
                await _storeContext.SaveChangesAsync();

                return RedirectToAction("Edit", new { productId = product.ProductId });
            }
            return View(pro);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int productId)
        {

            Product pro = await _storeContext.Products
                                            .Include(p => p.ProductVariants).ThenInclude(v => v.Promotions)
                                            .Include(p => p.ProductImages)
                                            .FirstOrDefaultAsync(p => p.ProductId.Equals(productId));
            if (pro == null)
                return RedirectToAction("Index");
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
        public async Task<IActionResult> Edit(ProductWrite pro)
        {
            string er = "";
            if (!string.IsNullOrEmpty(pro.Varriants[0].Sku))
            {
                for (int i = 0; i < pro.Varriants.Count; i++)
                {
                    var v = pro.Varriants[i];
                    for (int j = i + 1; j < pro.Varriants.Count; j++)
                    {
                        if (v.Sku.Equals(pro.Varriants[j].Sku))
                        {
                            er = "Trùng mã SKU: " + v.Sku;
                            return await RedirectEdit((int)pro.ProductId, er);
                        }
                    }
                    v.ProductId = pro.ProductId;
                    ProductVariant prv = await _storeContext.ProductVariants.FindAsync(v.Sku);
                    if (prv != null)
                    {
                        ProductVariant updatedVariant = mapper.Map<ProductVariant>(v);
                        _storeContext.Entry(prv).CurrentValues.SetValues(updatedVariant);
                    }
                    else
                    {
                        ProductVariant newVariant = mapper.Map<ProductVariant>(v);
                        bool c = await ExistSKU(newVariant.Sku);
                        if (c)
                        {
                            er = "Trùng mã SKU: " + newVariant.Sku;
                            return await RedirectEdit((int)pro.ProductId, er);
                        }
                        _storeContext.ProductVariants.Add(newVariant);
                        await _storeContext.SaveChangesAsync();

                    }
                }
            }


            if (pro.Images != null)
            {
                if (pro.ImagesOrder.Any() && pro.ImagesOrder[0] != null)
                {
                    pro.ImagesOrder = System.Text.Json.JsonSerializer.Deserialize<List<string>>(pro.ImagesOrder[0]);
                }
                var old_img = _storeContext.ProductImages.Where(i => i.ProductId.Equals(pro.ProductId));
                foreach (var img in old_img)
                {
                    await firebaseStorage.RemoveImageAsync(img.Name);
                }
                _storeContext.RemoveRange(old_img);
                await _storeContext.SaveChangesAsync();
                List<string> image = await UploadImage(pro.Images, pro.ImagesOrder, (int)pro.ProductId);
                foreach (string i in image)
                {
                    ProductImage pi = new ProductImage()
                    {
                        Name = i,
                        ProductId = pro.ProductId
                    };
                    _storeContext.ProductImages.Add(pi);
                    await _storeContext.SaveChangesAsync();
                }
            }
            Product pr = await _storeContext.Products.FindAsync(pro.ProductId);
            pr.Name = pro.Name;
            pr.CategoryId = pro.CategoryId;
            pr.Description = pro.Description;
            if (!string.IsNullOrEmpty(pro.Varriants[0].Sku) && pro.IsActive)
                pr.IsActive = pro.IsActive;
            else
            {
                pr.IsActive = false;
            }
            _storeContext.Entry(pr).State = EntityState.Modified;
            await _storeContext.SaveChangesAsync();


            return RedirectToAction("Edit", new { productId = pro.ProductId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int productId)
        {
            using var transaction = await _storeContext.Database.BeginTransactionAsync();
            try
            {
                var product = await _storeContext.Products
                    .Include(p => p.ProductVariants)
                    .Include(p => p.ProductImages)
                    .FirstOrDefaultAsync(p => p.ProductId == productId);

                if (product == null)
                {
                    return BadRequest(new { message = "Không thể xoá. Hãy chuyển sang ngừng bán" });
                }

                _storeContext.ProductVariants.RemoveRange(product.ProductVariants);

                foreach (var item in product.ProductImages)
                {
                    await firebaseStorage.RemoveImageAsync(item.Name);
                }

                _storeContext.ProductImages.RemoveRange(product.ProductImages);

                _storeContext.Products.Remove(product);

                await _storeContext.SaveChangesAsync();

                await transaction.CommitAsync();

                return Ok(new { redirectToUrl = Url.Action("Index") });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(new { message = "Không thể xoá. Hãy chuyển sang ngừng bán" });
            }
        }
        [HttpPost]
        public async Task<IActionResult> RemoveVariant(string? sku)
        {
            if (string.IsNullOrEmpty(sku))
                return NotFound();

            ProductVariant variant = await _storeContext.ProductVariants.FindAsync(sku);
            if (variant == null)
                return NotFound();
            try
            {
                _storeContext.ProductVariants.Remove(variant);
                await _storeContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            return Ok();
        }

        private async Task<IActionResult> RedirectEdit(int proId, string errorMessage)
        {


            Product pro = await _storeContext.Products
                                            .Include(p => p.ProductVariants)
                                            .Include(p => p.ProductImages)
                                            .FirstOrDefaultAsync(p => p.ProductId.Equals(proId));
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
            ViewBag.Errors = errorMessage;
            return View("Edit", proWrite);
        }

        private async Task<List<string>> UploadImage(List<IFormFile> f, List<string> fOrder, int proId)
        {
            List<string> rs = new List<string>();
            if (!fOrder.Any())
            {
                for (int i = 0; i < f.Count; i++)
                {
                    if (f[i].Length > 0)
                    {
                        using (var stream = new MemoryStream())
                        {
                            await f[i].CopyToAsync(stream);
                            stream.Position = 0;
                            string fileName = proId.ToString() + "_" + i.ToString() + ".jpg";
                            var image = await firebaseStorage.UploadImageAsync(stream, fileName);
                            rs.Add(fileName);
                        }
                    }
                }
                return rs;
            }
            foreach (var file in f)
            {
                if (file.Length > 0)
                {
                    using (var stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream);
                        stream.Position = 0;
                        string fileName = Path.GetFileName(file.FileName);

                        for (int i = 0; i < fOrder.Count; i++)
                        {
                            if (fOrder[i].Equals(fileName))
                                fileName = proId.ToString() + "_" + i.ToString() + ".jpg";
                        }
                        var image = await firebaseStorage.UploadImageAsync(stream, fileName);
                        rs.Add(fileName);
                    }
                }
            }
            return rs;
        }

        private async Task<bool> ExistSKU(string sku)
        {
            var rs = await _storeContext.ProductVariants.FindAsync(sku);
            return rs != null;
        }

    }
}
