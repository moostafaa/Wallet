using System;
using MediatR;

namespace WalletService.Application.Commands.Wallet
{
    public class CompleteTopUpCommand : IRequest<bool>
    {
        public Guid RequestId { get; set; }
        public string TransactionReference { get; set; }
    }
}