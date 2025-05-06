using System;

namespace WalletService.Application.Models.DTOs
{
    public class WalletDto
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public string Currency { get; set; }
        public decimal Balance { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}