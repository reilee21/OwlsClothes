using Net.payOS.Types;

namespace Payment
{
    public interface IPaymentServices
    {
        Task<CreatePaymentResult> CreatePaymentLink(PaymentData data);
        Task<PaymentLinkInformation> GetPaymentInfomation(int transactionId);
        Task<PaymentLinkInformation> CancelPayment(int transactionId);

    }
}
