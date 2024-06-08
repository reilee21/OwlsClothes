using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Owls.Models
{
    public partial class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string? Name { get; set; }
        public string? Tag { get; set; }
        public int? ParentCate { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
