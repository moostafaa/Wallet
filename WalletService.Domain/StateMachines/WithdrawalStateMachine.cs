using System;
using WalletService.Domain.Events;

namespace WalletService.Domain.StateMachines
{
    public enum WithdrawalState
    {
        Requested,
        Approved,
        Processing,
        Success,
        Failed,
        Rejected
    }
    
    public class WithdrawalStateMachine
    {
        private WithdrawalState _currentState;
        private readonly Guid _requestId;
        
        public WithdrawalStateMachine(Guid requestId, WithdrawalState initialState = WithdrawalState.Requested)
        {
            _requestId = requestId;
            _currentState = initialState;
        }
        
        public WithdrawalState CurrentState => _currentState;
        
        public WithdrawalStateChangedEvent TransitionTo(WithdrawalState newState, string reason)
        {
            if (!CanTransitionTo(newState))
            {
                throw new InvalidOperationException(
                    $"Cannot transition from {_currentState} to {newState}");
            }
            
            var previousState = _currentState;
            _currentState = newState;
            
            return new WithdrawalStateChangedEvent(
                _requestId,
                previousState.ToString(),
                newState.ToString(),
                reason);
        }
        
        private bool CanTransitionTo(WithdrawalState newState)
        {
            return _currentState switch
            {
                WithdrawalState.Requested => newState is WithdrawalState.Approved or WithdrawalState.Rejected,
                WithdrawalState.Approved => newState is WithdrawalState.Processing,
                WithdrawalState.Processing => newState is WithdrawalState.Success or WithdrawalState.Failed,
                _ => false
            };
        }
    }
}