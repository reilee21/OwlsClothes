using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Net.payOS;

namespace Payment
{
    public static class RegisterServices
    {
        public static IServiceCollection AddPaymentDI(this IServiceCollection services, IConfiguration configuration)
        {
            PayOS payOS = new PayOS(configuration["PaymentConfig:PAYOS_CLIENT_ID"],
                            configuration["PaymentConfig:PAYOS_API_KEY"],
                            configuration["PaymentConfig:PAYOS_CHECKSUM_KEY"]);
            services.AddSingleton(payOS);

            services.AddScoped<IPaymentServices, PaymentServices>();
            return services;
        }
    }
}
