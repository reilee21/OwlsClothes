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

        public async Task<bool> CheckOut(List<CartItem> carts, CheckOutModel model, string userId)
        {
            double total = 0;
            foreach (var item in carts)
            {
                total += (double)(item.Quantity * item.Price);
                bool check = await CheckQuantity(item.Sku, item.Quantity);
                if (!check)
                {
                    return false;
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
                SubTotal = total,
                ShippingFee = shoppingfee,
                CustomerName = model.CutomerName,
                CustomerPhone = model.PhoneNumber,
                Address = model.Address + ", " + model.Ward + ", " + model.Dicstrict + ", " + model.City
            };
            try
            {
                _storeContext.Orders.Add(order);
            }
            catch
            {
                return false;
            }
            foreach (var item in carts)
            {
                OrderDetail detail = new OrderDetail()
                {
                    Sku = item.Sku,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    OrderId = order.OrderId,

                };
                // Remove after test
                /* var productVariant = await _storeContext.ProductVariants.SingleOrDefaultAsync(pv => pv.Sku == item.Sku);
                 productVariant.Quantity -= item.Quantity;
                 _storeContext.ProductVariants.Update(productVariant);*/

                try
                {
                    _storeContext.OrderDetails.Add(detail);
                }
                catch { return false; }
            }
            await _storeContext.SaveChangesAsync();
            return true;
        }

        public async Task<PageListResponse<Order>> GetCustomerOrder(string userId, int pageSize, int page)
        {
            var ods = await _storeContext.Orders.Include(o => o.OrderDetails).ThenInclude(od => od.ProductVariant).ThenInclude(od => od.Product)
                .Where(o => o.UserId == userId).ToListAsync();
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
            if (pr == null) return false;
            return quantity <= pr.Quantity;
        }

    }
}
