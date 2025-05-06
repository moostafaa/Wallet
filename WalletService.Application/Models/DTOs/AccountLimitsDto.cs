using System;
using System.Collections.Generic;

namespace WalletService.Application.Models.DTOs
{
    public class AccountLimitsDto
    {
        public Guid AccountId { get; set; }
        public string RiskLevel { get; set; }
        public bool IsFrozen { get; set; }
        public DateTime? FrozenAt { get; set; }
        public string FrozenReason { get; set; }
        public Dictionary<string, decimal> TransactionLimits { get; set; }
        public Dictionary<string, decimal> DailyLimits { get; set; }
        public Dictionary<string, decimal> MonthlyLimits { get; set; }
        public Dictionary<string, decimal> CurrentDailyUsage { get; set; }
        public Dictionary<string, decimal> CurrentMonthlyUsage { get; set; }
    }
}