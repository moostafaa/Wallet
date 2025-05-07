using System;
using WalletService.Domain.Events;

namespace WalletService.Domain.StateMachines
{
    public enum TopUpState
    {
        Initiated,
        Processing,
        Success,
        Failed,
        Canceled
    }
    
    public class TopUpStateMachine
    {
        private TopUpState _currentState;
        private readonly Guid _requestId;
        
        public TopUpStateMachine(Guid requestId, TopUpState initialState = TopUpState.Initiated)
        {
            _requestId = requestId;
            _currentState = initialState;
        }
        
        public TopUpState CurrentState => _currentState;
        
        public TopUpStateChangedEvent TransitionTo(TopUpState newState, string reason)
        {
            if (!CanTransitionTo(newState))
            {
                throw new InvalidOperationException(
                    $"Cannot transition from {_currentState} to {newState}");
            }
            
            var previousState = _currentState;
            _currentState = newState;
            
            return new TopUpStateChangedEvent(
                _requestId,
                previousState.ToString(),
                newState.ToString(),
                reason);
        }
        
        private bool CanTransitionTo(TopUpState newState)
        {
            return _currentState switch
            {
                TopUpState.Initiated => newState is TopUpState.Processing or TopUpState.Canceled,
                TopUpState.Processing => newState is TopUpState.Success or TopUpState.Failed,
                _ => false
            };
        }
    }
}