namespace Owls.Services.Mail
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toAddress, string subject, string body);
    }
}
