using System;
using MediatR;

namespace WalletService.Application.Commands.Wallet
{
    public class CreateAutoTopUpRuleCommand : IRequest<Guid>
    {
        public Guid WalletId { get; set; }
        public decimal TriggerAmount { get; set; }
        public decimal TopUpAmount { get; set; }
        public string Source { get; set; }
    }
}