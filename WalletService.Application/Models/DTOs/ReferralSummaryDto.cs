using System;
using System.Collections.Generic;

namespace WalletService.Application.Models.DTOs
{
    public class ReferralSummaryDto
    {
        public Guid UserId { get; set; }
        public string ReferralCode { get; set; }
        public int TotalReferrals { get; set; }
        public decimal TotalEarnings { get; set; }
        public string CurrencyCode { get; set; }
        public List<ReferralBonusDto> PendingBonuses { get; set; }
        public List<ReferralBonusDto> PaidBonuses { get; set; }
    }
    
    public class ReferralBonusDto
    {
        public Guid Id { get; set; }
        public Guid ReferredUserId { get; set; }
        public string ReferredUserName { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? PaidAt { get; set; }
    }
}