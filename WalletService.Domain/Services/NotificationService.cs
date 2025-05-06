using System;
using System.Linq;
using System.Threading.Tasks;
using WalletService.Domain.Interfaces;
using WalletService.Domain.Models;

namespace WalletService.Domain.Services
{
    public abstract class NotificationService : INotificationService
    {
        protected readonly IEmailService _emailService;
        protected readonly ISmsService _smsService;

        protected NotificationService(
            IEmailService emailService,
            ISmsService smsService)
        {
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _smsService = smsService ?? throw new ArgumentNullException(nameof(smsService));
        }

        public abstract Task SendTransactionNotificationAsync(TransactionNotification notification);
        public abstract Task SendAccountNotificationAsync(AccountNotification notification);
        public abstract Task SendSecurityNotificationAsync(SecurityNotification notification);
        public abstract Task SendMarketingNotificationAsync(MarketingNotification notification);

        protected virtual async Task SendEmailNotificationAsync(string email, string subject, string body)
        {
            if (string.IsNullOrEmpty(email) || !_emailService.ValidateEmailAddress(email))
            {
                throw new ArgumentException("Invalid email address", nameof(email));
            }

            await _emailService.SendEmailAsync(email, subject, body);
        }

        protected virtual async Task SendSmsNotificationAsync(string phoneNumber, string message)
        {
            if (string.IsNullOrEmpty(phoneNumber) || !_smsService.ValidatePhoneNumber(phoneNumber))
            {
                throw new ArgumentException("Invalid phone number", nameof(phoneNumber));
            }

            await _smsService.SendSmsAsync(phoneNumber, message);
        }

        protected virtual bool ShouldSendViaChannel(NotificationBase notification, NotificationChannel channel)
        {
            return notification.Channels?.Contains(channel.ToString()) ?? false;
        }

        protected virtual string FormatCurrency(decimal amount, string currency)
        {
            return $"{amount:N2} {currency}";
        }
    }
}