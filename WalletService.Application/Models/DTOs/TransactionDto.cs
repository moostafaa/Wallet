using System;

namespace WalletService.Application.Models.DTOs
{
    public class TransactionDto
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public Guid? SourceWalletId { get; set; }
        public Guid? DestinationWalletId { get; set; }
        public string Source { get; set; }
        public string Reference { get; set; }
        public DateTime Timestamp { get; set; }
        public string Status { get; set; }
    }
}