using System;
namespace WalletService.Domain.Events
{
    public abstract class StateTransitionEvent : DomainEvent
    {
        public Guid EntityId { get; }
        public string FromState { get; }
        public string ToState { get; }
        public string Reason { get; }
        protected StateTransitionEvent(Guid entityId, string fromState, string toState, string reason = null)
        {
            EntityId = entityId;
            FromState = fromState;
            ToState = toState;
            Reason = reason;
        }
    }
}