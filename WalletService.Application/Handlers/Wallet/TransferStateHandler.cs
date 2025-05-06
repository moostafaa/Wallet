using System;
using System.Threading.Tasks;
using WalletService.Domain.Common;
using WalletService.Domain.Events;
using WalletService.Domain.StateMachines;
namespace WalletService.Domain.EventHandlers
{
    public class TransferStateHandler : IStateTransitionEventHandler<TransferState>
    {
        private readonly TransferState _stateTransition;
        public TransferStateHandler()
        {
            _stateTransition = new TransferState();
        }
        public bool CanHandle(object domainEvent)
        {
            return domainEvent switch
            {
                FundsTransferredEvent => true,
                TransferCompletedEvent => true,
                TransferFailedEvent => true,
                _ => false
            };
        }
        public Task HandleTransitionAsync(object domainEvent, TransferState currentState, TransferState newState)
        {
            _stateTransition.ValidateTransition(currentState, newState);
            switch (domainEvent)
            {
                case FundsTransferredEvent:
                    if (newState != TransferState.Initiated)
                        throw new InvalidOperationException("New transfer must transition to Initiated state");
                    break;
                case TransferCompletedEvent:
                    if (newState != TransferState.Completed)
                        throw new InvalidOperationException("Completed transfer must transition to Completed state");
                    break;
                case TransferFailedEvent:
                    if (newState != TransferState.Failed)
                        throw new InvalidOperationException("Failed transfer must transition to Failed state");
                    break;
            }
            return Task.CompletedTask;
        }
    }
}