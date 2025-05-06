using System;

namespace WalletService.Application.Models.DTOs
{
    public class DisputeDto
    {
        public Guid Id { get; set; }
        public Guid TransactionId { get; set; }
        public Guid WalletId { get; set; }
        public string Reason { get; set; }
        public string Evidence { get; set; }
        public string Status { get; set; }
        public string AdminNotes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public string ResolvedBy { get; set; }
        public Guid? RefundTransactionId { get; set; }
    }
}