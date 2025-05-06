using System;
using System.Collections.Generic;

namespace WalletService.Application.Models.DTOs
{
    public class AccountSummaryDto
    {
        public Guid AccountId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string KycLevel { get; set; }
        public decimal DailyTransactionLimit { get; set; }
        public decimal MonthlyTransactionLimit { get; set; }
        public decimal DailyTransactionUsage { get; set; }
        public decimal MonthlyTransactionUsage { get; set; }
        public List<WalletSummaryDto> Wallets { get; set; }
        public List<TransactionDto> RecentTransactions { get; set; }
    }
}