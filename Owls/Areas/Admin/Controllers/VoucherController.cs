using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Owls.DTOs;
using Owls.Models;

namespace Owls.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class VoucherController : Controller
    {
        private readonly OwlStoreContext context;

        public VoucherController(OwlStoreContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index(int? page, string? search)
        {
            if (page == null)
                page = 1;
            ViewBag.Nav = "Voucher";

            IEnumerable<Voucher> vouchers = new List<Voucher>();

            if (!string.IsNullOrEmpty(search))
            {
                vouchers = await context.Vouchers.Where(v => v.Name.ToUpper().Contains(search.Trim().ToUpper()))
                                        .ToListAsync();
            }
            else
            {
                vouchers = await context.Vouchers.ToListAsync();
            }

            ViewBag.Pager = new Pager() { CurrentPage = (int)page, ItemsPerPage = 10, TotalItems = vouchers.Count() };
            vouchers = vouchers.Skip(10 * ((int)page - 1)).Take(10);

            return View(vouchers);
        }


        [HttpPost]
        public async Task<IActionResult> Create(Voucher newVoucher)
        {
            string newCode = newVoucher.Name.ToUpper();
            var find = await context.Vouchers.FirstOrDefaultAsync(v => v.Name.ToUpper().Equals(newCode));

            if (find != null)
                return BadRequest("Đã có voucher");

            try
            {
                newVoucher.Name = newCode;
                if (newVoucher.Type.Equals("percent"))
                {
                    newVoucher.Type = "Phần trăm";
                }
                else
                {
                    newVoucher.Type = "Cố định";
                }
                context.Vouchers.Add(newVoucher);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest("Lỗi hệ thống");
            }

            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var vch = await context.Vouchers.FindAsync(id);
            return Ok(vch);
        }
        [HttpPut]
        public async Task<IActionResult> Update(Voucher voucher)
        {
            var vch = await context.Vouchers.FindAsync(voucher.Id);
            if (vch == null)
                return NotFound("Không tìm thấy voucher");
            if (voucher.Type.Equals("percent"))
            {
                voucher.Type = "Phần trăm";
            }
            else
            {
                voucher.Type = "Cố định";
            }
            voucher.Name = voucher.Name.ToUpper();
            try
            {
                context.Entry(vch).CurrentValues.SetValues(voucher);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest("Lỗi hệ thống");
            }
            return Ok(voucher);
        }


        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var vch = await context.Vouchers.FindAsync(id);
            if (vch == null)
                return NotFound("Không tìm thấy");
            try
            {
                context.Vouchers.Remove(vch);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest("Không thể xoá");
            }
            return Ok(vch);
        }
    }
}
