namespace Owls.Helper
{
	public static class OrderStatus
	{
		public static Dictionary<int, string> Status = new Dictionary<int, string>()
		{
			{ 0, "Đã đặt hàng" },
			{ 1, "Đang giao hàng" },
			{ 2, "Đã giao hàng" }
		};
	}
}
