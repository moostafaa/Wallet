using System;

namespace WalletService.Domain.Events
{
    public class TopUpWebhookReceivedEvent : DomainEvent
    {
        public Guid RequestId { get; }
        public string TransactionReference { get; }
        public string Status { get; }
        public string Currency { get; }
        public decimal Amount { get; }
        public string PaymentMethod { get; }
        public DateTime ProcessedAt { get; }
        
        public TopUpWebhookReceivedEvent(
            Guid requestId,
            string transactionReference,
            string status,
            string currency,
            decimal amount,
            string paymentMethod,
            DateTime processedAt)
        {
            RequestId = requestId;
            TransactionReference = transactionReference;
            Status = status;
            Currency = currency;
            Amount = amount;
            PaymentMethod = paymentMethod;
            ProcessedAt = processedAt;
        }
    }
    
    public class WebhookValidationFailedEvent : DomainEvent
    {
        public string WebhookType { get; }
        public string Reason { get; }
        public string RawPayload { get; }
        
        public WebhookValidationFailedEvent(
            string webhookType,
            string reason,
            string rawPayload)
        {
            WebhookType = webhookType;
            Reason = reason;
            RawPayload = rawPayload;
        }
    }
}