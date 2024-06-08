using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Owls.Models
{
    public partial class Color
    {

        [Key]
        public int ColorId { get; set; }
        public string? ColorName { get; set; }

        public virtual ICollection<ProductVariant> ProductVariants { get; set; }
    }
}
