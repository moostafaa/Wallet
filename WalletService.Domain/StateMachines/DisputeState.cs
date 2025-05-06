using WalletService.Domain.Common;

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
    
    public class DisputeState : StateTransition<DisputeState>
    {
        protected override void ConfigureTransitions()
        {
            Allow(DisputeState.Open, DisputeState.UnderReview);
            Allow(DisputeState.Open, DisputeState.Cancelled);
            Allow(DisputeState.UnderReview, DisputeState.Resolved);
            Allow(DisputeState.UnderReview, DisputeState.Escalated);
            Allow(DisputeState.Escalated, DisputeState.Resolved);
            Allow(DisputeState.Escalated, DisputeState.UnderReview);
            Allow(DisputeState.Resolved, DisputeState.UnderReview);
        }
    }
}
