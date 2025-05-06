using System;
using MediatR;

namespace WalletService.Application.Commands.Wallet
{
    public class InitiateTopUpCommand : IRequest<Guid>
    {
        public Guid WalletId { get; set; }
        public decimal Amount { get; set; }
        public string Source { get; set; }
        public string ExternalReference { get; set; }
    }
}