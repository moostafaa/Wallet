using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WalletService.Application.Models.DTOs;
using WalletService.Application.Queries.Wallet;
using WalletService.Infrastructure.ReadModels;

namespace WalletService.Application.Handlers.Queries
{
    public class GetWalletQueryHandler : IRequestHandler<GetWalletQuery, WalletDto>
    {
        private readonly WalletReadModel _walletReadModel;
        
        public GetWalletQueryHandler(WalletReadModel walletReadModel)
        {
            _walletReadModel = walletReadModel ?? throw new ArgumentNullException(nameof(walletReadModel));
        }
        
        public async Task<WalletDto> Handle(GetWalletQuery request, CancellationToken cancellationToken)
        {
            return await _walletReadModel.GetWalletAsync(request.WalletId);
        }
    }
}