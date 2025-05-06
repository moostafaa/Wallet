using System;
using MediatR;

namespace WalletService.Application.Commands.Wallet
{
    public class TransferFundsCommand : IRequest<bool>
    {
        public Guid SourceWalletId { get; set; }
        public Guid DestinationWalletId { get; set; }
        public decimal Amount { get; set; }
        public string Reference { get; set; }
    }
}