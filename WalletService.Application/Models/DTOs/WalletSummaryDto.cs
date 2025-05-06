using System;
using System.Collections.Generic;

namespace WalletService.Application.Models.DTOs
{
    public class WalletSummaryDto
    {
        public Guid WalletId { get; set; }
        public string Currency { get; set; }
        public decimal Balance { get; set; }
        public decimal PendingBalance { get; set; }
        public bool IsActive { get; set; }
        public List<TransactionDto> RecentTransactions { get; set; }
        public decimal TotalIncoming30Days { get; set; }
        public decimal TotalOutgoing30Days { get; set; }
    }
}