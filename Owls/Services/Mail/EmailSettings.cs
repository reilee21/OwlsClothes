namespace Owls.Services.Mail
{
    public class EmailSettings
    {
        public string FromAddress { get; set; }
        public string Password { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public bool EnableSsl { get; set; }
    }
}
