using System;
using MediatR;

namespace WalletService.Application.Commands.Wallet
{
    public class CompletePaymentCommand : IRequest<bool>
    {
        public Guid PaymentId { get; set; }
    }
}