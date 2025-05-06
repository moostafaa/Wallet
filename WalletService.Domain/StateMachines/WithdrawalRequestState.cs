
using WalletService.Domain.Common;

namespace WalletService.Domain.StateMachines
{
    public enum WithdrawalState
    {
        Requested,
        Approved,
        Processing,
        Success,
        Failed,
        Rejected
    }
    
    public class WithdrawalRequestState : StateTransition<WithdrawalState>
    {
        protected override void ConfigureTransitions()
        {
            Allow(WithdrawalState.Requested, WithdrawalState.Approved);
            Allow(WithdrawalState.Requested, WithdrawalState.Rejected);
            Allow(WithdrawalState.Approved, WithdrawalState.Processing);
            Allow(WithdrawalState.Processing, WithdrawalState.Success);
            Allow(WithdrawalState.Processing, WithdrawalState.Failed);
            Allow(WithdrawalState.Failed, WithdrawalState.Rejected);
        }
    }
}
