using Owls.DTOs;
using Owls.Models;

namespace Owls.Repositories.OrderRepos
{
    public interface IOrderRepos
    {
        public Task<Order> CheckOut(List<CartItem> carts, CheckOutModel model, string userId);
        public Task<PageListResponse<Order>> GetCustomerOrder(string userId, int pageSize, int page);
        public Task<Order> GetOrderDetail(string orderId);

        public Task<bool> HandleCallBack(CallBackPayment callBackPayment);
    }
}
