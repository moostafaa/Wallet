using System;

namespace WalletService.Application.Models.Webhooks
{
    public class TopUpWebhookPayload
    {
        public Guid RequestId { get; set; }
        public string TransactionReference { get; set; }
        public string Status { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime ProcessedAt { get; set; }
        public ErrorDetails Error { get; set; }
    }
    
    public class ErrorDetails
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string Reason { get; set; }
    }
}