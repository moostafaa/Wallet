using System;
using MediatR;
using WalletService.Domain.Aggregates.WalletAccountAggregate;

namespace WalletService.Application.Commands.WalletAccount
{
    public class UpdateKycLevelCommand : IRequest<bool>
    {
        public Guid AccountId { get; set; }
        public KycLevel Level { get; set; }
        public string DocumentReference { get; set; }
    }
}