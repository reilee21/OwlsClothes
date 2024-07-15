using System.ComponentModel.DataAnnotations;

namespace Owls.Models
{
    public partial class ProductVariant
    {
        public ProductVariant()
        {
            Promotions = new HashSet<Promotion>();
        }

        [Key]
        public string Sku { get; set; } = null!;
        public int? ProductId { get; set; }
        public double? Cost { get; set; }
        public string? Size { get; set; }
        public int? ColorId { get; set; }
        public double? SalePrice { get; set; }
        public int? Quantity { get; set; }

        public virtual Color? Color { get; set; }
        public virtual Product? Product { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Promotion> Promotions { get; set; }

    }
}
