using System;
using MediatR;

namespace WalletService.Application.Commands.Wallet
{
    public class InitiatePaymentCommand : IRequest<Guid>
    {
        public Guid BuyerWalletId { get; set; }
        public Guid MerchantWalletId { get; set; }
        public decimal Amount { get; set; }
        public string OrderReference { get; set; }
        public string Description { get; set; }
    }
}