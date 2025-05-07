using System;
using WalletService.Domain.Events;

namespace WalletService.Domain.StateMachines
{
    public enum TransactionState
    {
        Pending,
        Completed,
        Failed,
        Reversed
    }
    
    public class WalletTransactionStateMachine
    {
        private TransactionState _currentState;
        private readonly Guid _transactionId;
        
        public WalletTransactionStateMachine(Guid transactionId, TransactionState initialState = TransactionState.Pending)
        {
            _transactionId = transactionId;
            _currentState = initialState;
        }
        
        public TransactionState CurrentState => _currentState;
        
        public WalletTransactionStateChangedEvent TransitionTo(TransactionState newState, string reason)
        {
            if (!CanTransitionTo(newState))
            {
                throw new InvalidOperationException(
                    $"Cannot transition from {_currentState} to {newState}");
            }
            
            var previousState = _currentState;
            _currentState = newState;
            
            return new WalletTransactionStateChangedEvent(
                _transactionId,
                previousState.ToString(),
                newState.ToString(),
                reason);
        }
        
        private bool CanTransitionTo(TransactionState newState)
        {
            return _currentState switch
            {
                TransactionState.Pending => newState is TransactionState.Completed or TransactionState.Failed,
                TransactionState.Completed => newState is TransactionState.Reversed,
                _ => false
            };
        }
    }
}