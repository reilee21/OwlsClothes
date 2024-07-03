using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Owls.Models;

namespace Owls.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "AdminOrStaff")]

    public class OthersController : Controller
    {
        private readonly OwlStoreContext owlStoreContext;

        public OthersController(OwlStoreContext owlStoreContext)
        {
            this.owlStoreContext = owlStoreContext;
        }

        public IActionResult Index()
        {
            ViewBag.Nav = "Others";
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> SearchCity(string city)
        {
            if (string.IsNullOrEmpty(city))
            {
                return Ok(await owlStoreContext.ShippingFees.ToListAsync());

            }
            string s = city.ToLower().Trim();
            var rs = await owlStoreContext.ShippingFees.Where(c => c.City.ToLower().Contains(s)).ToListAsync();
            if (!rs.Any())
                return NotFound();
            return Ok(rs);

        }

        [HttpGet]
        public async Task<IActionResult> GetShip(int id)
        {
            var rs = await owlStoreContext.ShippingFees.FindAsync(id);
            return Ok(rs);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateShippingFee([FromForm] ShippingFee model)
        {
            if (ModelState.IsValid)
            {
                var shippingFee = await owlStoreContext.ShippingFees.FindAsync(model.Id);
                if (shippingFee == null) return NotFound();

                shippingFee.City = model.City;
                shippingFee.Fee = model.Fee;
                await owlStoreContext.SaveChangesAsync();
                return Ok();
            }
            return BadRequest(ModelState);

        }
        [HttpPost]
        public async Task<IActionResult> AddColor(string colorName)
        {
            var c = await owlStoreContext.Colors.FirstOrDefaultAsync(c => c.ColorName.ToLower() == colorName.ToLower());
            if (c != null) return BadRequest(c);
            Color cl = new Color() { ColorName = colorName };
            owlStoreContext.Colors.Add(cl);
            await owlStoreContext.SaveChangesAsync();
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> EditColor(Color color)
        {
            var c = await owlStoreContext.Colors
                .FirstOrDefaultAsync(c => c.ColorId != color.ColorId && c.ColorName.ToLower() == color.ColorName.ToLower());
            if (c != null) return BadRequest(c);
            Color cl = await owlStoreContext.Colors.FindAsync(color.ColorId);
            if (cl != null)
            {
                cl.ColorName = color.ColorName;
                owlStoreContext.Entry(cl).State = EntityState.Modified;
                await owlStoreContext.SaveChangesAsync();
            }
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> RemoveColor(Color color)
        {

            Color cl = await owlStoreContext.Colors.FindAsync(color.ColorId);
            if (cl != null)
            {
                try
                {
                    owlStoreContext.Colors.Remove(cl);
                    await owlStoreContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }
            }
            return Ok();
        }
    }
}
