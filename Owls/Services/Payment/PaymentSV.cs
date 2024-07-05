using Net.payOS.Types;
using Owls.Models;
using Payment;

namespace Owls.Services.Payment
{
    public class PaymentSV : IPayment
    {
        private readonly IConfiguration configuration;
        private readonly IPaymentServices payment;
        public PaymentSV(IConfiguration configuration, IPaymentServices paymentServices)
        {
            this.configuration = configuration;
            this.payment = paymentServices;
        }

        public async Task<CreatePaymentResult> CreatePaymentLink(Order order)
        {

            int transId = (int)order.TransactionId;

            PaymentData paymentData = new PaymentData
            (
                //amount: (int)order.Total,
                amount: 5000,
                orderCode: transId,
                items: null,
                description: $"Thanh toan Owls Clothes",
                returnUrl: configuration["PaymentConfig:SUCCESS_URL_CALLBACK"],
                cancelUrl: configuration["PaymentConfig:CANCEL_URL_CALLBACK"],
                expiredAt: (int)DateTimeOffset.Now.AddMinutes(20).ToUnixTimeSeconds()
            );
            try
            {
                var paymentlink = await payment.CreatePaymentLink(paymentData);
                return paymentlink;
            }
            catch (Exception ex)
            {
                throw new Exception("------------------------\n Failed to create payment link" + ex.Message);
            }
        }

        public async Task<PaymentLinkInformation> GetPaymentInfomation(int transactionId)
        {
            try
            {
                var paymentinfo = await payment.GetPaymentInfomation(transactionId);
                return paymentinfo;
            }
            catch (Exception ex)
            {
                throw new Exception("------------------------\n Failed to get payment information" + ex.Message);
            }
        }
        public Task<PaymentLinkInformation> CancelPayment(int transactionId)
        {
            throw new NotImplementedException();
        }
    }
}
