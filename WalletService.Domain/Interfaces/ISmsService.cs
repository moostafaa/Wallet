using System.Threading.Tasks;

namespace WalletService.Domain.Interfaces
{
    public interface ISmsService
    {
        Task SendSmsAsync(string phoneNumber, string message);
        Task SendBulkSmsAsync(string[] phoneNumbers, string message);
        bool ValidatePhoneNumber(string phoneNumber);
    }
}