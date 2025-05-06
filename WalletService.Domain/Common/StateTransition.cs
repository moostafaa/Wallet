```csharp
using System;
using System.Collections.Generic;

namespace WalletService.Domain.Common
{
    public abstract class StateTransition<TState> where TState : Enum
    {
        private readonly Dictionary<TState, HashSet<TState>> _allowedTransitions;
        
        protected StateTransition()
        {
            _allowedTransitions = new Dictionary<TState, HashSet<TState>>();
            ConfigureTransitions();
        }
        
        protected abstract void ConfigureTransitions();
        
        protected void Allow(TState from, TState to)
        {
            if (!_allowedTransitions.ContainsKey(from))
            {
                _allowedTransitions[from] = new HashSet<TState>();
            }
            
            _allowedTransitions[from].Add(to);
        }
        
        public bool CanTransitionTo(TState from, TState to)
        {
            return _allowedTransitions.ContainsKey(from) && _allowedTransitions[from].Contains(to);
        }
        
        public void ValidateTransition(TState from, TState to)
        {
            if (!CanTransitionTo(from, to))
            {
                throw new InvalidOperationException($"Cannot transition from {from} to {to}");
            }
        }
    }
}
```