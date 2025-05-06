using System;
using System.Threading.Tasks;
using WalletService.Domain.Events;
using WalletService.Domain.Interfaces;
using WalletService.Infrastructure.Persistence;

namespace WalletService.Infrastructure.Services
{
    public class NotificationService
    {
        private readonly ReadDbContext _readDbContext;
        
        public NotificationService(ReadDbContext readDbContext)
        {
            _readDbContext = readDbContext;
        }
        
        public async Task HandleTransactionNotification(TransactionNotificationEvent notification)
        {
            var account = await _readDbContext.Accounts.FindAsync(notification.AccountId);
            if (account == null)
                return;
                
            // Send email notification
            if (notification.NotificationChannel.Contains("email") && !string.IsNullOrEmpty(account.Email))
            {
                await SendEmailNotification(
                    account.Email,
                    $"Transaction Notification: {notification.Type}",
                    $"Amount: {notification.Amount} {notification.CurrencyCode}\n{notification.Description}");
            }
            
            // Send SMS notification
            if (notification.NotificationChannel.Contains("sms") && !string.IsNullOrEmpty(account.PhoneNumber))
            {
                await SendSmsNotification(
                    account.PhoneNumber,
                    $"Transaction: {notification.Type} {notification.Amount} {notification.CurrencyCode}");
            }
        }
        
        private async Task SendEmailNotification(string email, string subject, string body)
        {
            // Implement email sending logic
            await Task.CompletedTask;
        }
        
        private async Task SendSmsNotification(string phoneNumber, string message)
        {
            // Implement SMS sending logic
            await Task.CompletedTask;
        }
    }
}