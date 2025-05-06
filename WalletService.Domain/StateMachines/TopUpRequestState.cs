using WalletService.Domain.Common;

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
    
    public class TopUpRequestState : StateTransition<TopUpState>
    {
        protected override void ConfigureTransitions()
        {
            Allow(TopUpState.Initiated, TopUpState.Processing);
            Allow(TopUpState.Initiated, TopUpState.Canceled);
            Allow(TopUpState.Processing, TopUpState.Success);
            Allow(TopUpState.Processing, TopUpState.Failed);
            Allow(TopUpState.Failed, TopUpState.Canceled);
        }
    }
}