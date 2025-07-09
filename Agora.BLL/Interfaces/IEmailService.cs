using Microsoft.AspNetCore.Http;

namespace Agora.BLL.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
        Task SendEmailWithAttachmentAsync(string toEmail, string subject, string body, IFormFile? attachment);
    }
}
