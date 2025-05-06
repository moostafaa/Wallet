
using WalletService.Domain.Common;

namespace WalletService.Domain.StateMachines
{
    public enum TransferState
    {
        Initiated,
        Completed,
        Failed,
        Reversed
    }
    
    public class TransferState : StateTransition<TransferState>
    {
        protected override void ConfigureTransitions()
        {
            AAllow(TransferState.Initiated, TransferState.Completed);
            Allow(TransferState.Initiated, TransferState.Failed);
            Allow(TransferState.Completed, TransferState.Reversed);
            Allow(TransferState.Failed, TransferState.Reversed);
            Allow(TransferState.Reversed, TransferState.Completed);
        }
    }
}
