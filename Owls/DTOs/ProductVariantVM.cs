namespace Owls.DTOs
{
	public class ProductVariantVM
	{
		public int? ProductId { get; set; }
		public string Sku { get; set; } = null!;
		public double? Cost { get; set; }
		public string? Size { get; set; }
		public int? ColorId { get; set; }
		public double? SalePrice { get; set; }
		public int? Quantity { get; set; }
		public int? DicountId { get; set;}
	}
    public class ProductVariantRVM
    {
        public string Sku { get; set; } = null!;
        public string? Size { get; set; }
        public string? ColorName { get; set; }
        public double? SalePrice { get; set; }
        public int? Quantity { get; set; }
        public int? Discount { get; set; }
    }
}
