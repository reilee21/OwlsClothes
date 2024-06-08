using Owls.DTOs;
using Owls.Models;

namespace Owls.Repositories.OrderRepos
{
	public interface IOrderRepos
	{
		public Task<bool> CheckOut(List<CartItem> carts, CheckOutModel model, string userId);
		public Task<IEnumerable<Order>> GetCustomerOrder(string userId);
		public Task<Order> GetOrderDetail(string orderId);
	}
}
