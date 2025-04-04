namespace E_Commerce.ModelHelpers
{
    public interface IMailService
    {
        Task<string> SendEmail(string mailTo, string subject, string body, IList<IFormFile>? attachments = null);
    }
}
