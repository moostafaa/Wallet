using System;
using System.Collections.Generic;

namespace WalletService.Application.Models.DTOs
{
    public class WalletAccountDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string KycLevel { get; set; }
        public DateTime CreatedAt { get; set; }
        public IEnumerable<Guid> WalletIds { get; set; }
    }
}