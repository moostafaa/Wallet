using System.Threading.Tasks;
using WalletService.Domain.Models;

namespace WalletService.Domain.Interfaces
{
    public interface INotificationService
    {
        Task SendTransactionNotificationAsync(TransactionNotification notification);
        Task SendAccountNotificationAsync(AccountNotification notification);
        Task SendSecurityNotificationAsync(SecurityNotification notification);
        Task SendMarketingNotificationAsync(MarketingNotification notification);
    }
}