using System.ComponentModel.DataAnnotations;

namespace Owls.DTOs
{
    public class CartItem
    {
        public string? Sku { get; set; }
        [Compare("StockQuantity", ErrorMessage = "Sản phẩm không còn đủ số lượng")]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
        public string? ProductName { get; set; }
        public string? Image { get; set; }
        public string? Size { get; set; }
        public int? Color { get; set; }
        public double? Price { get; set; }
        public double? Discount { get; set; } = 0;
        public int? StockQuantity { get; set; }


    }

}
