using System.Collections.Generic;

namespace WalletService.Domain.Services
{
    public static class NotificationTemplates
    {
        public static class Email
        {
            public const string TransactionConfirmation = "TransactionConfirmation";
            public const string SecurityAlert = "SecurityAlert";
            public const string AccountUpdate = "AccountUpdate";
            public const string MarketingCampaign = "MarketingCampaign";
        }

        public static class SMS
        {
            public const string TransactionAlert = "TransactionAlert";
            public const string SecurityWarning = "SecurityWarning";
            public const string AccountAlert = "AccountAlert";
            public const string MarketingMessage = "MarketingMessage";
        }

        public static class Locales
        {
            public static readonly Dictionary<string, string> SupportedLocales = new()
            {
                { "en", "English" },
                { "es", "Spanish" },
                { "fr", "French" },
                { "de", "German" }
            };
        }
    }
}