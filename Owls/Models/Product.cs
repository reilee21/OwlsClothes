using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Owls.Models
{
    public partial class Product
    {

        [Key]
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public int? CategoryId { get; set; }
        public string? Description { get; set; }
        public string? Imagethumbnail { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreateAt { get; set; }

        public virtual Category? Category { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }
        public virtual ICollection<ProductVariant> ProductVariants { get; set; }
    }
}
