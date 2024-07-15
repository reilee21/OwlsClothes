using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Owls.Models
{
    public class Promotion
    {
        public Promotion()
        {
            Products = new HashSet<ProductVariant>();
        }

        public int PromotionId { get; set; }
        [Required(ErrorMessage = "Không để trống")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Không để trống")]
        [Range(0, 100, ErrorMessage = "Không hợp lệ")]
        public double DiscountPercentage { get; set; }
        [Required(ErrorMessage = "Không để trống")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "Không để trống")]
        public DateTime EndDate { get; set; }

        public virtual ICollection<ProductVariant> Products { get; set; }

        [NotMapped]
        public List<string> SelectedProductSkus { get; set; }

    }
}
