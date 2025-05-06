using System.Threading.Tasks;
namespace WalletService.Domain.Common
{
    public interface IStateTransitionEventHandler<TState> where TState : Enum
    {
        Task HandleTransitionAsync(object domainEvent, TState currentState, TState newState);
        bool CanHandle(object domainEvent);
    }
}