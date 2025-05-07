using System;
using WalletService.Domain.Events;

namespace WalletService.Domain.StateMachines
{
    public enum TransferState
    {
        Initiated,
        Completed,
        Failed,
        Reversed
    }
    
    public class TransferStateMachine
    {
        private TransferState _currentState;
        private readonly Guid _transferId;
        
        public TransferStateMachine(Guid transferId, TransferState initialState = TransferState.Initiated)
        {
            _transferId = transferId;
            _currentState = initialState;
        }
        
        public TransferState CurrentState => _currentState;
        
        public TransferStateChangedEvent TransitionTo(TransferState newState, string reason)
        {
            if (!CanTransitionTo(newState))
            {
                throw new InvalidOperationException(
                    $"Cannot transition from {_currentState} to {newState}");
            }
            
            var previousState = _currentState;
            _currentState = newState;
            
            return new TransferStateChangedEvent(
                _transferId,
                previousState.ToString(),
                newState.ToString(),
                reason);
        }
        
        private bool CanTransitionTo(TransferState newState)
        {
            return _currentState switch
            {
                TransferState.Initiated => newState is TransferState.Completed or TransferState.Failed,
                TransferState.Completed => newState is TransferState.Reversed,
                _ => false
            };
        }
    }
}