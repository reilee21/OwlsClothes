namespace Owls.DTOs
{
    public class RevenueRs
    {
        public DateTime Date { get; set; }
        public double Amount { get; set; }
    }
    public class BestSellerProduct
    {
        public string ProductName { get; set; }
        public double Revenue { get; set; }
        public int Quantity { get; set; }
    }
}
