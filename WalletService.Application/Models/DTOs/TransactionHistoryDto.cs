using System;
using System.Collections.Generic;

namespace WalletService.Application.Models.DTOs
{
    public class TransactionHistoryDto
    {
        public List<TransactionDto> Transactions { get; set; }
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public decimal TotalIncoming { get; set; }
        public decimal TotalOutgoing { get; set; }
    }
}