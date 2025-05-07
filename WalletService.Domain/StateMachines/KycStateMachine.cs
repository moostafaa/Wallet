using System;
using WalletService.Domain.Events;

namespace WalletService.Domain.StateMachines
{
    public enum KycState
    {
        Submitted,
        Verified,
        Rejected
    }
    
    public class KycStateMachine
    {
        private KycState _currentState;
        private readonly Guid _accountId;
        
        public KycStateMachine(Guid accountId, KycState initialState = KycState.Submitted)
        {
            _accountId = accountId;
            _currentState = initialState;
        }
        
        public KycState CurrentState => _currentState;
        
        public KycStateChangedEvent TransitionTo(KycState newState, string documentReference)
        {
            if (!CanTransitionTo(newState))
            {
                throw new InvalidOperationException(
                    $"Cannot transition from {_currentState} to {newState}");
            }
            
            var previousState = _currentState;
            _currentState = newState;
            
            return new KycStateChangedEvent(
                _accountId,
                previousState.ToString(),
                newState.ToString(),
                documentReference);
        }
        
        private bool CanTransitionTo(KycState newState)
        {
            return _currentState switch
            {
                KycState.Submitted => newState is KycState.Verified or KycState.Rejected,
                _ => false
            };
        }
    }
}