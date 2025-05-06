using System;
using MediatR;
using WalletService.Domain.Aggregates.WalletAccountAggregate;

namespace WalletService.Application.Commands.WalletAccount
{
    public class CreateWalletAccountCommand : IRequest<Guid>
    {
        public string Name { get; set; }
        public AccountType Type { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PrimaryCurrency { get; set; }
    }
}