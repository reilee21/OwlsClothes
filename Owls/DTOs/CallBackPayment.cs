namespace Owls.DTOs
{
    public class CallBackPayment
    {
        public string Code { get; set; }
        public string Id { get; set; }
        public bool Cancel { get; set; }
        public string Status { get; set; }
        public string OrderCode { get; set; }
    }
}
