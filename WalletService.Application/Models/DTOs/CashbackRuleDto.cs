using System;

namespace WalletService.Application.Models.DTOs
{
    public class CashbackRuleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Percentage { get; set; }
        public decimal MinTransactionAmount { get; set; }
        public string CurrencyCode { get; set; }
        public decimal MaxCashbackAmount { get; set; }
        public bool IsActive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string[] EligibleTransactionTypes { get; set; }
    }
}