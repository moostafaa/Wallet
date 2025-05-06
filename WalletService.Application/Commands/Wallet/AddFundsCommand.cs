using System;
using MediatR;

namespace WalletService.Application.Commands.Wallet
{
    public class AddFundsCommand : IRequest<bool>
    {
        public Guid WalletId { get; set; }
        public decimal Amount { get; set; }
        public string Source { get; set; }
        public string Reference { get; set; }
    }
}