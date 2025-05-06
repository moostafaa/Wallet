using System;
using System.Threading.Tasks;
using WalletService.Domain.Common;
using WalletService.Domain.Events;
using WalletService.Domain.StateMachines;
namespace WalletService.Domain.EventHandlers
{
    public class TopUpRequestStateHandler : IStateTransitionEventHandler<TopUpState>
    {
        private readonly TopUpRequestState _stateTransition;
        public TopUpRequestStateHandler()
        {
            _stateTransition = new TopUpRequestState();
        }
        public bool CanHandle(object domainEvent)
        {
            return domainEvent switch
            {
                TopUpRequestCreatedEvent => true,
                TopUpCompletedEvent => true,
                _ => false
            };
        }
        public Task HandleTransitionAsync(object domainEvent, TopUpState currentState, TopUpState newState)
        {
            _stateTransition.ValidateTransition(currentState, newState);
            switch (domainEvent)
            {
                case TopUpRequestCreatedEvent:
                    if (newState != TopUpState.Processing)
                        throw new InvalidOperationException("New top-up request must transition to Processing state");
                    break;
                case TopUpCompletedEvent:
                    if (newState != TopUpState.Success)
                        throw new InvalidOperationException("Completed top-up must transition to Success state");
                    break;
            }
            return Task.CompletedTask;
        }
    }
}