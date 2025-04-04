
using Microsoft.Extensions.Options;

using System.Net.Mail;
using System.Net;

namespace E_Commerce.ModelHelpers
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;

        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task<string> SendEmail(string mailTo, string subject, string body, IList<IFormFile>? attachments = null)
        {
            try
            {
                var smtpClient = new SmtpClient(_mailSettings.Host)
                {
                    Port = _mailSettings.Port,
                    Credentials = new NetworkCredential(_mailSettings.Email, _mailSettings.Password),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(mailTo),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(mailTo);


                await smtpClient.SendMailAsync(mailMessage);

                return "Email sent successfully";
            }
            catch (Exception ex)
            {
                return $"Failed to send email: {ex.Message}";
            }
        }
    }
}
