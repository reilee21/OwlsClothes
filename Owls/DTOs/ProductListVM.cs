using Owls.Models;

namespace Owls.DTOs
{
	public class ProductListVM
	{
		public IEnumerable<ProductBaseInformation> Products { get; set; } = Enumerable.Empty<ProductBaseInformation>();
		public Pager PagingInfo { get; set; } = new ();
		public Filter Filter { get; set; } = new();
	}
}
