using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Owls.Models
{
    public partial class ProductImage
    {
        [Key]
        public int ImgId { get; set; }
        public int? ProductId { get; set; }
        public string? Name { get; set; }

        public virtual Product? Product { get; set; }
    }
}
