﻿using System.ComponentModel.DataAnnotations;

namespace Owls.Models
{
    public partial class Order
    {

        [Key]
        public string OrderId { get; set; } = null!;
        public string? UserId { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public string? Status { get; set; }
        public double? SubTotal { get; set; }
        public double? ShippingFee { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerPhone { get; set; }
        public string? Address { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public double Total => (SubTotal ?? 0) + (ShippingFee ?? 0);

    }
}
