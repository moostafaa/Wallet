using System;
using System.Collections.Generic;
using MediatR;
using WalletService.Application.Models.DTOs;

namespace WalletService.Application.Queries.Wallet
{
    public class GetWalletTransactionsQuery : IRequest<IEnumerable<TransactionDto>>
    {
        public Guid WalletId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 10;
    }
}