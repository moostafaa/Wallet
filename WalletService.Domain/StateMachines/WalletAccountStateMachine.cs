using System;
using WalletService.Domain.Events;

namespace WalletService.Domain.StateMachines
{
    public enum AccountState
    {
        Active,
        Suspended,
        Closed
    }
    
    public class WalletAccountStateMachine
    {
        private AccountState _currentState;
        private readonly Guid _accountId;
        
        public WalletAccountStateMachine(Guid accountId, AccountState initialState = AccountState.Active)
        {
            _accountId = accountId;
            _currentState = initialState;
        }
        
        public AccountState CurrentState => _currentState;
        
        public WalletAccountStateChangedEvent TransitionTo(AccountState newState, string reason)
        {
            if (!CanTransitionTo(newState))
            {
                throw new InvalidOperationException(
                    $"Cannot transition from {_currentState} to {newState}");
            }
            
            var previousState = _currentState;
            _currentState = newState;
            
            return new WalletAccountStateChangedEvent(
                _accountId,
                previousState.ToString(),
                newState.ToString(),
                reason);
        }
        
        private bool CanTransitionTo(AccountState newState)
        {
            return _currentState switch
            {
                AccountState.Active => newState is AccountState.Suspended or AccountState.Closed,
                AccountState.Suspended => newState is AccountState.Active or AccountState.Closed,
                _ => false
            };
        }
    }
}