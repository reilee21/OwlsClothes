using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Owls.DTOs;
using Owls.Models;

namespace Owls.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class PromotionController : Controller
    {
        private readonly OwlStoreContext context;

        public PromotionController(OwlStoreContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index(int? page, string? search)
        {
            if (page == null)
                page = 1;
            ViewBag.Nav = "Promotion";

            IEnumerable<Promotion> rs = new List<Promotion>();

            if (!string.IsNullOrEmpty(search))
            {
                rs = await context.Promotions.Where(v => v.Name.ToUpper().Contains(search.Trim().ToUpper()))
                                        .ToListAsync();
            }
            else
            {
                rs = await context.Promotions.ToListAsync();
            }

            ViewBag.Pager = new Pager() { CurrentPage = (int)page, ItemsPerPage = 10, TotalItems = rs.Count() };
            rs = rs.Skip(10 * ((int)page - 1)).Take(10);

            return View(rs);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Nav = "Promotion";

            var productVariants = context.ProductVariants
                    .Include(pv => pv.Product)
                    .Include(pv => pv.Color)
                    .Select(pv => new
                    {
                        pv.Sku,
                        DisplayName = $"{pv.Product.Name} {(string.IsNullOrEmpty(pv.Size) ? "" : " - " + pv.Size)} {(pv.Color != null ? " - " + pv.Color.ColorName : "")}"
                    }).ToList();

            ViewBag.ProductVariants = new SelectList(productVariants, "Sku", "DisplayName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Promotion promotion)
        {
            ViewBag.Nav = "Promotion";

            if (ModelState.IsValid)
            {
                var selectedProducts = context.ProductVariants
                    .Where(pv => promotion.SelectedProductSkus.Contains(pv.Sku))
                    .ToList();

                promotion.Products = new HashSet<ProductVariant>(selectedProducts);

                context.Add(promotion);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var productVariants = context.ProductVariants
                .Include(pv => pv.Product)
                .Include(pv => pv.Color)
                .Select(pv => new
                {
                    pv.Sku,
                    DisplayName = $"{pv.Product.Name} {(string.IsNullOrEmpty(pv.Size) ? "" : " - " + pv.Size)} {(pv.Color != null ? " - " + pv.Color.ColorName : "")}"
                }).ToList();

            ViewBag.ProductVariants = new MultiSelectList(productVariants, "Sku", "DisplayName");
            return View(promotion);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Nav = "Promotion";

            var promotion = context.Promotions
                .Include(p => p.Products).ThenInclude(v => v.Color)
                .Include(p => p.Products).ThenInclude(v => v.Product)
                .FirstOrDefault(p => p.PromotionId == id);

            if (promotion == null)
            {
                return NotFound();
            }

            var productVariants = context.ProductVariants
                .Include(pv => pv.Product)
                .Include(pv => pv.Color)
                .Select(pv => new
                {
                    pv.Sku,
                    DisplayName = $"{pv.Product.Name} {(string.IsNullOrEmpty(pv.Size) ? "" : " - " + pv.Size)} {(pv.ColorId.HasValue ? " - " + pv.Color.ColorName : "")}"
                }).ToList();

            ViewBag.ProductVariants = new SelectList(productVariants, "Sku", "DisplayName");

            return View(promotion);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Promotion promotion)
        {
            ViewBag.Nav = "Promotion";

            if (ModelState.IsValid)
            {
                try
                {
                    var existingPromotion = await context.Promotions
                        .Include(p => p.Products)
                        .FirstOrDefaultAsync(p => p.PromotionId == promotion.PromotionId);

                    if (existingPromotion == null)
                    {
                        return NotFound();
                    }

                    // Update promotion details
                    existingPromotion.Name = promotion.Name;
                    existingPromotion.DiscountPercentage = promotion.DiscountPercentage;
                    existingPromotion.StartDate = promotion.StartDate;
                    existingPromotion.EndDate = promotion.EndDate;


                    // Update selected products
                    existingPromotion.Products.Clear();
                    foreach (var sku in promotion.SelectedProductSkus)
                    {

                        var productVariant = context.ProductVariants.FirstOrDefault(pv => pv.Sku == sku);
                        if (productVariant != null)
                        {
                            existingPromotion.Products.Add(productVariant);
                        }
                    }
                    // Save changes
                    context.Update(existingPromotion);
                    await context.SaveChangesAsync();

                    return RedirectToAction("Edit", new { id = promotion.PromotionId });
                }
                catch (DbUpdateConcurrencyException)
                {
                    return View("Index");
                }
            }
            return View(promotion);
        }


        [HttpDelete]

        public async Task<IActionResult> Delete(int id)
        {
            var promotion = await context.Promotions.FindAsync(id);
            try
            {
                context.Entry(promotion).State = EntityState.Deleted;
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest("");
            }
            return Ok(promotion);
        }


    }
}
