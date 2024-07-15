using Microsoft.EntityFrameworkCore;
using Owls.DTOs;
using Owls.Helper;
using Owls.Models;

namespace Owls.Repositories.OrderRepos
{
    public class OrderRepos : IOrderRepos
    {
        private readonly OwlStoreContext _storeContext;

        public OrderRepos(OwlStoreContext storeContext)
        {
            _storeContext = storeContext;
        }

        public async Task<Order> CheckOut(List<CartItem> carts, CheckOutModel model, string userId)
        {
            double total = 0;
            foreach (var item in carts)
            {
                total += (double)(item.Quantity * item.Price * (1 - item.Discount / 100));
                bool check = await CheckQuantity(item.Sku, item.Quantity);
                if (!check)
                {
                    return null;
                }
            }

            int shoppingfee = await GetShippingFee(model.City);
            Order order = new Order()
            {
                UserId = userId,
                OrderId = Guid.NewGuid().ToString(),
                CreateAt = DateTime.Now,
                UpdateAt = DateTime.Now,
                Status = OrderStatus.Status[0],
                IsPaid = false,
                SubTotal = total,
                ShippingFee = shoppingfee,
                CustomerName = model.CutomerName,
                CustomerPhone = model.PhoneNumber,
                PaymentMethod = model.PaymentMethod,
                Address = model.Address + ", " + model.Ward + ", " + model.Dicstrict + ", " + model.City,
                VoucherDiscount = 0,
            };
            if (!string.IsNullOrEmpty(model.Voucher))
            {
                var voucher = await _storeContext.Vouchers.FirstOrDefaultAsync(v => v.Name.ToUpper().Equals(model.Voucher.ToUpper()));
                if (voucher == null || voucher.Quantity < 1 || !voucher.IsActive)
                    return null;
                try
                {
                    voucher.Quantity -= 1;
                    if (voucher.Quantity < 1)
                        voucher.IsActive = false;
                    _storeContext.Entry(voucher).State = EntityState.Modified;
                }
                catch (Exception ex)
                {
                    return null;
                }
                order.VoucherId = voucher.Id;
                if (voucher.Type == "Phần trăm")
                {
                    order.VoucherDiscount = order.SubTotal * voucher.Value / 100;
                }
                else
                {
                    order.VoucherDiscount = voucher.Value;
                }
            }

            if (model.PaymentMethod == PaymentMethod.BANK.ToString())
            {
                order.TransactionId = GenerateTransactionId();
            }
            order.OrderDetails = new HashSet<OrderDetail>();
            foreach (var item in carts)
            {
                OrderDetail detail = new OrderDetail()
                {
                    Sku = item.Sku,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    OrderId = order.OrderId,
                    Discount = item.Discount.HasValue ? item.Discount.Value : 0,
                };
                order.OrderDetails.Add(detail);
                var productVariant = await _storeContext.ProductVariants.SingleOrDefaultAsync(pv => pv.Sku == item.Sku);
                productVariant.Quantity -= item.Quantity;
                _storeContext.Entry(productVariant).State = EntityState.Modified;
            }


            try
            {
                _storeContext.Orders.Add(order);
                await _storeContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("----------------------------------------" + ex.InnerException?.Message);
                return null;
            }
            return order;
        }

        public async Task<PageListResponse<Order>> GetCustomerOrder(string userId, int pageSize, int page)
        {
            var ods = await _storeContext.Orders.Include(o => o.OrderDetails).ThenInclude(od => od.ProductVariant).ThenInclude(od => od.Product)
                .Where(o => o.UserId == userId).OrderByDescending(o => o.CreateAt).ToListAsync();
            var rs = ods.Skip((page - 1) * pageSize).Take(pageSize);
            if (rs != null)
            {
                foreach (var o in rs)
                {
                    foreach (var od in o.OrderDetails)
                    {
                        var colorId = od.ProductVariant.ColorId;
                        if (colorId != null)
                        {
                            var color = await _storeContext.Colors.FindAsync(colorId);
                            od.ProductVariant.Color = color;
                        }
                    }
                }

            }
            PageListResponse<Order> result = new PageListResponse<Order>
            {
                CurrentPage = page,
                ItemsPerPage = pageSize,
                TotalItems = ods.Count,
                Data = rs.ToList()
            };
            return result;
        }

        public async Task<Order> GetOrderDetail(string orderId)
        {
            var rs = await _storeContext.Orders.Include(p => p.OrderDetails).ThenInclude(od => od.ProductVariant).ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.OrderId.Equals(orderId));
            if (rs != null)
            {
                foreach (var od in rs.OrderDetails)
                {
                    var colorId = od.ProductVariant.ColorId;
                    if (colorId != null)
                    {
                        var color = await _storeContext.Colors.FindAsync(colorId);
                        od.ProductVariant.Color = color;
                    }
                }
            }

            return rs;
        }

        private async Task<int> GetShippingFee(string city)
        {
            var f = await _storeContext.ShippingFees.FirstOrDefaultAsync(p => p.City.Equals(city));
            return f != null ? f.Fee : 0;
        }

        private async Task<bool> CheckQuantity(string sku, int quantity)
        {
            var pr = await _storeContext.ProductVariants.FindAsync(sku);
            if (pr == null)
                return false;
            return quantity <= pr.Quantity;
        }
        public int GenerateTransactionId()
        {
            Random rand = new Random();
            int Id;
            bool valid = false;

            do
            {
                Id = rand.Next(100000, 9999999);
                var od = _storeContext.Orders.FirstOrDefault(d => d.TransactionId == Id);
                if (od == null)
                    valid = true;
            } while (!valid);

            return Id;
        }

        public async Task<bool> HandleCallBack(CallBackPayment callBackPayment)
        {
            if (callBackPayment.Code == "00")
            {
                int transcallback = int.Parse(callBackPayment.OrderCode);
                var od = await _storeContext.Orders.Include(od => od.OrderDetails)
                    .FirstOrDefaultAsync(o => o.TransactionId.HasValue && o.TransactionId.Value.Equals(transcallback));
                if (od == null)
                { return false; }

                if (callBackPayment.Cancel) // đơn huỷ 
                {
                    List<ProductVariant> updatedVariants = new List<ProductVariant>();

                    foreach (var pro in od.OrderDetails) // trả lại số lượng hàng
                    {
                        var pro_variant = await _storeContext.ProductVariants.FindAsync(pro.Sku);
                        pro_variant.Quantity += pro.Quantity;
                        updatedVariants.Add(pro_variant);

                    }
                    od.Status = OrderStatus.Status.GetValueOrDefault(3); // trạng thái đơn

                    if (od.VoucherId.HasValue)
                    {
                        var voucher = await _storeContext.Vouchers.FindAsync(od.VoucherId);
                        voucher.Quantity += 1;
                        _storeContext.Entry(voucher).State = EntityState.Modified;
                    }

                    _storeContext.Entry(od).State = EntityState.Modified;
                    _storeContext.ProductVariants.UpdateRange(updatedVariants);

                    await _storeContext.SaveChangesAsync();
                    return false;
                }
                od.IsPaid = true;
                _storeContext.Entry(od).State = EntityState.Modified;
                await _storeContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
