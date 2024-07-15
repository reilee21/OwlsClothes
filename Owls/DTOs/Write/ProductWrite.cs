using Owls.Models;
using System.ComponentModel.DataAnnotations;

namespace Owls.DTOs.Write
{
    public class ProductWrite
    {
        public int? ProductId { get; set; }
        [Required(ErrorMessage = "Phải đặt tên cho sản phẩm")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Phải có danh mục cho sản phẩm")]
        public int? CategoryId { get; set; }
        public string? Description { get; set; }
        public List<IFormFile>? Images { get; set; }
        public List<string>? ImagesOrder { get; set; }

        public List<ProVariant> Varriants { get; set; } = new List<ProVariant>() { new ProVariant() { Quantity = 1 } };

        public bool IsActive { get; set; } = false;
        public DateTime? CreateAt { get; set; }
        public List<string>? ImagesDisplay { get; set; }

    }

    public partial class ProVariant
    {
        [Required(ErrorMessage = "Không được để trống")]
        public string Sku { get; set; } = null!;
        public int? ProductId { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
        public double? Cost { get; set; }
        public string? Size { get; set; }
        public int? ColorId { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Giá bán phải lớn hơn 0")]
        public double? SalePrice { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng phải từ 0 trở lên")]
        public int? Quantity { get; set; }
        public virtual ICollection<Promotion> Promotions { get; set; }

    }

}
