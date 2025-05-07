using System;
using WalletService.Domain.Events;

namespace WalletService.Domain.StateMachines
{
    public enum DisputeState
    {
        Open,
        UnderReview,
        Resolved,
        Escalated,
        Cancelled
    }
    
    public class DisputeStateMachine
    {
        private DisputeState _currentState;
        private readonly Guid _disputeId;
        
        public DisputeStateMachine(Guid disputeId, DisputeState initialState = DisputeState.Open)
        {
            _disputeId = disputeId;
            _currentState = initialState;
        }
        
        public DisputeState CurrentState => _currentState;
        
        public DisputeStateChangedEvent TransitionTo(DisputeState newState, string reason)
        {
            if (!CanTransitionTo(newState))
            {
                throw new InvalidOperationException(
                    $"Cannot transition from {_currentState} to {newState}");
            }
            
            var previousState = _currentState;
            _currentState = newState;
            
            return new DisputeStateChangedEvent(
                _disputeId,
                previousState.ToString(),
                newState.ToString(),
                reason);
        }
        
        private bool CanTransitionTo(DisputeState newState)
        {
            return _currentState switch
            {
                DisputeState.Open => newState is DisputeState.UnderReview or DisputeState.Cancelled,
                DisputeState.UnderReview => newState is DisputeState.Resolved or DisputeState.Escalated,
                DisputeState.Escalated => newState is DisputeState.Resolved,
                _ => false
            };
        }
    }
}