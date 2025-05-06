using System;
using MediatR;

namespace WalletService.Application.Commands.Wallet
{
    public class DeactivateWalletCommand : IRequest<bool>
    {
        public Guid WalletId { get; set; }
    }
}