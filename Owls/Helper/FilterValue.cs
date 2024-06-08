namespace Owls.Helper
{
	public class FilterValue
	{
		public static Dictionary<string, string> PriceTag = new Dictionary<string, string>()
		{
			{ "under_100k", "Dưới 100.000đ" },
			{ "100k_200k", "100.000đ - 200.000đ" },
			{ "200k_300k", "200.000đ - 300.000đ" },
			{ "300k_500k", "300.000đ - 500.000đ" },
			{ "over_500k", "Trên 500.000đ" }
		};
	}
}
