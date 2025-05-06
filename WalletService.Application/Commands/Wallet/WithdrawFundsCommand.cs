using System;
using MediatR;

namespace WalletService.Application.Commands.Wallet
{
    public class WithdrawFundsCommand : IRequest<bool>
    {
        public Guid WalletId { get; set; }
        public decimal Amount { get; set; }
        public string Destination { get; set; }
        public string Reference { get; set; }
    }
}