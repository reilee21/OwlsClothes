namespace Owls.Models
{
    public class Voucher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public double Value { get; set; }
        public bool IsActive { get; set; }
        public int Quantity { get; set; }
        public ICollection<Order> Orders { get; set; }

    }

}
