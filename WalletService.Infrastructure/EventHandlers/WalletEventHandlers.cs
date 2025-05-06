using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WalletService.Domain.Events;
using WalletService.Infrastructure.Persistence;

namespace WalletService.Infrastructure.EventHandlers
{
    public class WalletEventHandlers : 
        INotificationHandler<WalletCreatedEvent>,
        INotificationHandler<FundsAddedEvent>,
        INotificationHandler<FundsTransferredEvent>,
        INotificationHandler<FundsReceivedEvent>,
        INotificationHandler<FundsWithdrawnEvent>,
        INotificationHandler<WalletDeactivatedEvent>
    {
        private readonly ReadDbContext _readDbContext;
        
        public WalletEventHandlers(ReadDbContext readDbContext)
        {
            _readDbContext = readDbContext;
        }
        
        public async Task Handle(WalletCreatedEvent notification, CancellationToken cancellationToken)
        {
            var walletReadModel = new WalletReadModel
            {
                Id = notification.WalletId,
                AccountId = notification.AccountId,
                Currency = notification.CurrencyCode,
                Balance = 0,
                IsActive = true,
                CreatedAt = notification.Timestamp
            };
            
            await _readDbContext.Wallets.AddAsync(walletReadModel, cancellationToken);
            await _readDbContext.SaveChangesAsync(cancellationToken);
        }
        
        public async Task Handle(FundsAddedEvent notification, CancellationToken cancellationToken)
        {
            var wallet = await _readDbContext.Wallets.FindAsync(notification.WalletId);
            if (wallet != null)
            {
                wallet.Balance = notification.NewBalance;
                
                var transaction = new TransactionReadModel
                {
                    Id = notification.TransactionId,
                    Type = "TopUp",
                    Amount = notification.Amount,
                    Currency = notification.CurrencyCode,
                    SourceWalletId = null,
                    DestinationWalletId = notification.WalletId,
                    Source = notification.Source,
                    Reference = notification.Reference,
                    Timestamp = notification.Timestamp,
                    Status = "Completed"
                };
                
                await _readDbContext.Transactions.AddAsync(transaction, cancellationToken);
                await _readDbContext.SaveChangesAsync(cancellationToken);
            }
        }
        
        public async Task Handle(FundsTransferredEvent notification, CancellationToken cancellationToken)
        {
            var sourceWallet = await _readDbContext.Wallets.FindAsync(notification.SourceWalletId);
            if (sourceWallet != null)
            {
                sourceWallet.Balance = notification.NewBalance;
                
                var transaction = new TransactionReadModel
                {
                    Id = notification.TransactionId,
                    Type = "Transfer",
                    Amount = -notification.Amount,
                    Currency = notification.CurrencyCode,
                    SourceWalletId = notification.SourceWalletId,
                    DestinationWalletId = notification.DestinationWalletId,
                    Source = "wallet",
                    Reference = notification.Reference,
                    Timestamp = notification.Timestamp,
                    Status = "Completed"
                };
                
                await _readDbContext.Transactions.AddAsync(transaction, cancellationToken);
                await _readDbContext.SaveChangesAsync(cancellationToken);
            }
        }
        
        public async Task Handle(FundsReceivedEvent notification, CancellationToken cancellationToken)
        {
            var wallet = await _readDbContext.Wallets.FindAsync(notification.WalletId);
            if (wallet != null)
            {
                wallet.Balance = notification.NewBalance;
                
                var transaction = new TransactionReadModel
                {
                    Id = notification.TransactionId,
                    Type = "Transfer",
                    Amount = notification.Amount,
                    Currency = notification.CurrencyCode,
                    SourceWalletId = notification.SourceWalletId,
                    DestinationWalletId = notification.WalletId,
                    Source = "wallet",
                    Reference = notification.Reference,
                    Timestamp = notification.Timestamp,
                    Status = "Completed"
                };
                
                await _readDbContext.Transactions.AddAsync(transaction, cancellationToken);
                await _readDbContext.SaveChangesAsync(cancellationToken);
            }
        }
        
        public async Task Handle(FundsWithdrawnEvent notification, CancellationToken cancellationToken)
        {
            var wallet = await _readDbContext.Wallets.FindAsync(notification.WalletId);
            if (wallet != null)
            {
                wallet.Balance = notification.NewBalance;
                
                var transaction = new TransactionReadModel
                {
                    Id = notification.TransactionId,
                    Type = "Withdrawal",
                    Amount = -notification.Amount,
                    Currency = notification.CurrencyCode,
                    SourceWalletId = notification.WalletId,
                    DestinationWalletId = null,
                    Source = notification.Destination,
                    Reference = notification.Reference,
                    Timestamp = notification.Timestamp,
                    Status = "Completed"
                };
                
                await _readDbContext.Transactions.AddAsync(transaction, cancellationToken);
                await _readDbContext.SaveChangesAsync(cancellationToken);
            }
        }
        
        public async Task Handle(WalletDeactivatedEvent notification, CancellationToken cancellationToken)
        {
            var wallet = await _readDbContext.Wallets.FindAsync(notification.WalletId);
            if (wallet != null)
            {
                wallet.IsActive = false;
                await _readDbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}