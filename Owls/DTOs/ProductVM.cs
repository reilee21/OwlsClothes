using System.ComponentModel.DataAnnotations;

namespace Owls.DTOs
{
    public class ProductBaseInformation
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        public string? Name { get; set; }
        public int? CategoryId { get; set; }
        public string? Description { get; set; }
        public string? Imagethumbnail { get; set; }
        public bool? IsActive { get; set; }
        public int? TotalQuantity { get; set; }
        public List<ProductVariantRVM>? ProductVariants { get; set; }
        public double? TotalDiscount { get; set; } = 0;
        public int? Price { get; set; }
    }

    public class ProductReadVM : ProductBaseInformation
    {
        public List<string>? Images { get; set; }

    }

}
