using Net.payOS;
using Net.payOS.Types;

namespace Payment
{
    public class PaymentServices : IPaymentServices
    {
        private readonly PayOS payOS;

        public PaymentServices(PayOS payOS)
        {
            this.payOS = payOS;
        }


        public async Task<CreatePaymentResult> CreatePaymentLink(PaymentData data)
        {
            try
            {
                CreatePaymentResult createPayment = await payOS.createPaymentLink(data);
                return createPayment;
            }
            catch (Exception exception)
            {
                throw new ApplicationException("Failed to Create Payment Link", exception);
            }

        }

        public async Task<PaymentLinkInformation> GetPaymentInfomation(int transactionId)
        {
            try
            {
                PaymentLinkInformation paymentLinkInformation = await payOS.getPaymentLinkInformation(transactionId);
                return paymentLinkInformation;
            }
            catch (Exception exception)
            {

                throw new ApplicationException("Failed to Get Payment Infomation", exception);
            }
        }
        public async Task<PaymentLinkInformation> CancelPayment(int transactionId)
        {
            try
            {
                PaymentLinkInformation paymentLinkInformation = await payOS.cancelPaymentLink(transactionId);
                return paymentLinkInformation;
            }
            catch (Exception exception)
            {
                throw new ApplicationException("Failed to Cancel Payment", exception);

            }
        }
    }
}
