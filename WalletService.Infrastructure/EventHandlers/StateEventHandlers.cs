using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WalletService.Domain.Events;
using WalletService.Infrastructure.Persistence;

namespace WalletService.Infrastructure.EventHandlers
{
    public class StateEventHandlers :
        INotificationHandler<DisputeStateChangedEvent>,
        INotificationHandler<KycStateChangedEvent>,
        INotificationHandler<TopUpStateChangedEvent>,
        INotificationHandler<TransferStateChangedEvent>,
        INotificationHandler<WalletAccountStateChangedEvent>,
        INotificationHandler<WalletTransactionStateChangedEvent>,
        INotificationHandler<WithdrawalStateChangedEvent>
    {
        private readonly ReadDbContext _readDbContext;
        
        public StateEventHandlers(ReadDbContext readDbContext)
        {
            _readDbContext = readDbContext;
        }
        
        public async Task Handle(DisputeStateChangedEvent notification, CancellationToken cancellationToken)
        {
            var dispute = await _readDbContext.Disputes.FindAsync(notification.DisputeId);
            if (dispute != null)
            {
                dispute.Status = notification.NewState;
                await _readDbContext.SaveChangesAsync(cancellationToken);
            }
        }
        
        public async Task Handle(KycStateChangedEvent notification, CancellationToken cancellationToken)
        {
            var account = await _readDbContext.Accounts.FindAsync(notification.AccountId);
            if (account != null)
            {
                account.KycStatus = notification.NewState;
                account.KycDocumentReference = notification.DocumentReference;
                await _readDbContext.SaveChangesAsync(cancellationToken);
            }
        }
        
        public async Task Handle(TopUpStateChangedEvent notification, CancellationToken cancellationToken)
        {
            var topUp = await _readDbContext.TopUpRequests.FindAsync(notification.RequestId);
            if (topUp != null)
            {
                topUp.Status = notification.NewState;
                await _readDbContext.SaveChangesAsync(cancellationToken);
            }
        }
        
        public async Task Handle(TransferStateChangedEvent notification, CancellationToken cancellationToken)
        {
            var transfer = await _readDbContext.Transfers.FindAsync(notification.TransferId);
            if (transfer != null)
            {
                transfer.Status = notification.NewState;
                await _readDbContext.SaveChangesAsync(cancellationToken);
            }
        }
        
        public async Task Handle(WalletAccountStateChangedEvent notification, CancellationToken cancellationToken)
        {
            var account = await _readDbContext.Accounts.FindAsync(notification.AccountId);
            if (account != null)
            {
                account.Status = notification.NewState;
                await _readDbContext.SaveChangesAsync(cancellationToken);
            }
        }
        
        public async Task Handle(WalletTransactionStateChangedEvent notification, CancellationToken cancellationToken)
        {
            var transaction = await _readDbContext.Transactions.FindAsync(notification.TransactionId);
            if (transaction != null)
            {
                transaction.Status = notification.NewState;
                await _readDbContext.SaveChangesAsync(cancellationToken);
            }
        }
        
        public async Task Handle(WithdrawalStateChangedEvent notification, CancellationToken cancellationToken)
        {
            var withdrawal = await _readDbContext.WithdrawalRequests.FindAsync(notification.RequestId);
            if (withdrawal != null)
            {
                withdrawal.Status = notification.NewState;
                await _readDbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}