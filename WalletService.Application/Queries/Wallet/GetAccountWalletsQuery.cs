using System;
using System.Collections.Generic;
using MediatR;
using WalletService.Application.Models.DTOs;

namespace WalletService.Application.Queries.Wallet
{
    public class GetAccountWalletsQuery : IRequest<IEnumerable<WalletDto>>
    {
        public Guid AccountId { get; set; }
    }
}