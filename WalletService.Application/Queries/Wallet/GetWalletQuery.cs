using System;
using MediatR;
using WalletService.Application.Models.DTOs;

namespace WalletService.Application.Queries.Wallet
{
    public class GetWalletQuery : IRequest<WalletDto>
    {
        public Guid WalletId { get; set; }
    }
}