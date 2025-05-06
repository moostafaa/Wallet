using System;

namespace WalletService.Domain.Models
{
    public abstract class NotificationBase
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string[] Channels { get; set; }
        public DateTime CreatedAt { get; set; }
        public NotificationPriority Priority { get; set; }
        public string Locale { get; set; }
    }

    public class TransactionNotification : NotificationBase
    {
        public string TransactionType { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
        public Guid TransactionId { get; set; }
        public decimal NewBalance { get; set; }
    }

    public class AccountNotification : NotificationBase
    {
        public string Type { get; set; }
        public string Message { get; set; }
        public string ActionRequired { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }

    public class SecurityNotification : NotificationBase
    {
        public string AlertType { get; set; }
        public string DeviceInfo { get; set; }
        public string Location { get; set; }
        public string IpAddress { get; set; }
        public bool RequiresAction { get; set; }
    }

    public class MarketingNotification : NotificationBase
    {
        public string CampaignId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string[] Tags { get; set; }
        public DateTime? ValidUntil { get; set; }
    }

    public enum NotificationPriority
    {
        Low,
        Normal,
        High,
        Critical
    }

    public enum NotificationChannel
    {
        Email,
        SMS,
        PushNotification,
        InApp
    }
}