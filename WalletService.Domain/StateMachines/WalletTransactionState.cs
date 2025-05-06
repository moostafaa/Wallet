using WalletService.Domain.Common;

namespace WalletService.Domain.StateMachines
{
    public enum TransactionState
    {
        Pending,
        Completed,
        Failed,
        Reversed
    }
    
    public class WalletTransactionState : StateTransition<TransactionState>
    {
        protected override void ConfigureTransitions()
        {
            Allow(TransactionState.Pending, TransactionState.Completed);
            Allow(TransactionState.Pending, TransactionState.Failed);
            Allow(TransactionState.Completed, TransactionState.Reversed);
            Allow(TransactionState.Failed, TransactionState.Reversed);
        }
    }
}
