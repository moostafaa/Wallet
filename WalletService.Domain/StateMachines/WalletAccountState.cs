using WalletService.Domain.Common;

namespace WalletService.Domain.StateMachines
{
    public enum AccountState
    {
        Active,
        Suspended,
        Closed
    }
    
    public class WalletAccountState : StateTransition<AccountState>
    {
        protected override void ConfigureTransitions()
        {
            Allow(AccountState.Active, AccountState.Suspended);
            Allow(AccountState.Active, AccountState.Closed);
            Allow(AccountState.Suspended, AccountState.Active);
            Allow(AccountState.Suspended, AccountState.Closed);
            Allow(AccountState.Closed, AccountState.Active);
        }
    }
}