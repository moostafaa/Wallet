using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WalletService.Application.Models.DTOs;
using WalletService.Infrastructure.Persistence;

namespace WalletService.Infrastructure.ReadModels
{
    public class WalletAccountReadModel
    {
        private readonly ReadDbContext _dbContext;
        
        public WalletAccountReadModel(ReadDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        
        public async Task<WalletAccountDto> GetAccountAsync(Guid accountId)
        {
            var account = await _dbContext.Accounts
                .Include(a => a.WalletIds)
                .FirstOrDefaultAsync(a => a.Id == accountId);
                
            if (account == null)
                return null;
                
            return new WalletAccountDto
            {
                Id = account.Id,
                Name = account.Name,
                Type = account.Type,
                Status = account.Status,
                KycLevel = account.KycLevel,
                CreatedAt = account.CreatedAt,
                WalletIds = account.WalletIds.Select(w => w.WalletId)
            };
        }
    }
}