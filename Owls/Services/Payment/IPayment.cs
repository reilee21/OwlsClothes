using Net.payOS.Types;
using Owls.Models;

namespace Owls.Services.Payment
{
    public interface IPayment
    {
        Task<CreatePaymentResult> CreatePaymentLink(Order order);
        Task<PaymentLinkInformation> GetPaymentInfomation(int transactionId);
        Task<PaymentLinkInformation> CancelPayment(int transactionId);
    }
}
