using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Owls.Services.Mail
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }
        public async Task SendEmailAsync(string toAddress, string subject, string body)
        {
            try
            {
                var message = new MailMessage();
                message.From = new MailAddress(_emailSettings.FromAddress);
                message.To.Add(toAddress);
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient(_emailSettings.SmtpHost))
                {
                    smtp.Port = _emailSettings.SmtpPort;
                    smtp.Credentials = new NetworkCredential(_emailSettings.FromAddress, _emailSettings.Password);
                    smtp.EnableSsl = _emailSettings.EnableSsl;

                    await smtp.SendMailAsync(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
