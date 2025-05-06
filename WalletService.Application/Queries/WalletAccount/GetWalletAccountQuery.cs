using System;
using MediatR;
using WalletService.Application.Models.DTOs;

namespace WalletService.Application.Queries.WalletAccount
{
    public class GetWalletAccountQuery : IRequest<WalletAccountDto>
    {
        public Guid AccountId { get; set; }
    }
}