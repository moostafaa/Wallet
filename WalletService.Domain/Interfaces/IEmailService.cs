using System.Threading.Tasks;

namespace WalletService.Domain.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
        Task SendEmailWithTemplateAsync(string to, string templateName, object templateData);
        Task SendBulkEmailAsync(string[] recipients, string subject, string body);
        bool ValidateEmailAddress(string email);
    }
}