using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WalletService.Application.Models.DTOs;
using WalletService.Infrastructure.Persistence;

namespace WalletService.Infrastructure.ReadModels
{
    public class WalletReadModel
    {
        private readonly ReadDbContext _dbContext;
        
        public WalletReadModel(ReadDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        
        public async Task<WalletDto> GetWalletAsync(Guid walletId)
        {
            return await _dbContext.Wallets
                .Where(w => w.Id == walletId)
                .Select(w => new WalletDto
                {
                    Id = w.Id,
                    AccountId = w.AccountId,
                    Currency = w.Currency,
                    Balance = w.Balance,
                    IsActive = w.IsActive,
                    CreatedAt = w.CreatedAt
                })
                .FirstOrDefaultAsync();
        }
        
        public async Task<IEnumerable<WalletDto>> GetAccountWalletsAsync(Guid accountId)
        {
            return await _dbContext.Wallets
                .Where(w => w.AccountId == accountId)
                .Select(w => new WalletDto
                {
                    Id = w.Id,
                    AccountId = w.AccountId,
                    Currency = w.Currency,
                    Balance = w.Balance,
                    IsActive = w.IsActive,
                    CreatedAt = w.CreatedAt
                })
                .ToListAsync();
        }
        
        public async Task<IEnumerable<TransactionDto>> GetWalletTransactionsAsync(
            Guid walletId,
            DateTime? fromDate,
            DateTime? toDate,
            int skip,
            int take)
        {
            var query = _dbContext.Transactions
                .Where(t => t.SourceWalletId == walletId || t.DestinationWalletId == walletId);
                
            if (fromDate.HasValue)
                query = query.Where(t => t.Timestamp >= fromDate.Value);
                
            if (toDate.HasValue)
                query = query.Where(t => t.Timestamp <= toDate.Value);
                
            return await query
                .OrderByDescending(t => t.Timestamp)
                .Skip(skip)
                .Take(take)
                .Select(t => new TransactionDto
                {
                    Id = t.Id,
                    Type = t.Type,
                    Amount = t.Amount,
                    Currency = t.Currency,
                    SourceWalletId = t.SourceWalletId,
                    DestinationWalletId = t.DestinationWalletId,
                    Source = t.Source,
                    Reference = t.Reference,
                    Timestamp = t.Timestamp,
                    Status = t.Status
                })
                .ToListAsync();
        }
    }
}