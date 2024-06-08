using System.ComponentModel.DataAnnotations;

namespace Owls.Models
{
    public partial class OrderDetail
    {
        [Key]
        public int OrderDetailId { get; set; }
        public string Sku { get; set; } = null!;
        public string OrderId { get; set; } = null!;
        public double? Price { get; set; }
        public int? Quantity { get; set; }

        public virtual Order Order { get; set; } = null!;
        public virtual ProductVariant ProductVariant { get; set; } = null!;


    }
}
