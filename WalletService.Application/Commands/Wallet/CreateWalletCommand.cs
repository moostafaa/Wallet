using System;
using MediatR;

namespace WalletService.Application.Commands.Wallet
{
    public class CreateWalletCommand : IRequest<Guid>
    {
        public Guid AccountId { get; set; }
        public string CurrencyCode { get; set; }
    }
}