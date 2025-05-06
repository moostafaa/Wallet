using System;
using System.Threading.Tasks;
using WalletService.Domain.Common;
using WalletService.Domain.Events;
using WalletService.Domain.StateMachines;
namespace WalletService.Domain.EventHandlers
{
    public class WithdrawalRequestStateHandler : IStateTransitionEventHandler<WithdrawalState>
    {
        private readonly WithdrawalRequestState _stateTransition;
        public WithdrawalRequestStateHandler()
        {
            _stateTransition = new WithdrawalRequestState();
        }
        public bool CanHandle(object domainEvent)
        {
            return domainEvent switch
            {
                WithdrawalRequestCreatedEvent => true,
                WithdrawalRequestApprovedEvent => true,
                WithdrawalRequestRejectedEvent => true,
                _ => false
            };
        }
        public Task HandleTransitionAsync(object domainEvent, WithdrawalState currentState, WithdrawalState newState)
        {
            _stateTransition.ValidateTransition(currentState, newState);
            switch (domainEvent)
            {
                case WithdrawalRequestCreatedEvent:
                    if (newState != WithdrawalState.Requested)
                        throw new InvalidOperationException("New withdrawal request must transition to Requested state");
                    break;
                case WithdrawalRequestApprovedEvent:
                    if (newState != WithdrawalState.Approved)
                        throw new InvalidOperationException("Approved withdrawal must transition to Approved state");
                    break;
                case WithdrawalRequestRejectedEvent:
                    if (newState != WithdrawalState.Rejected)
                        throw new InvalidOperationException("Rejected withdrawal must transition to Rejected state");
                    break;
            }
            return Task.CompletedTask;
        }
    }
}