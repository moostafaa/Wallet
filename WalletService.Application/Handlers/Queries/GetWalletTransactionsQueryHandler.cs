using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WalletService.Application.Models.DTOs;
using WalletService.Application.Queries.Wallet;
using WalletService.Infrastructure.ReadModels;

namespace WalletService.Application.Handlers.Queries
{
    public class GetWalletTransactionsQueryHandler : IRequestHandler<GetWalletTransactionsQuery, IEnumerable<TransactionDto>>
    {
        private readonly WalletReadModel _walletReadModel;
        
        public GetWalletTransactionsQueryHandler(WalletReadModel walletReadModel)
        {
            _walletReadModel = walletReadModel ?? throw new ArgumentNullException(nameof(walletReadModel));
        }
        
        public async Task<IEnumerable<TransactionDto>> Handle(
            GetWalletTransactionsQuery request,
            CancellationToken cancellationToken)
        {
            return await _walletReadModel.GetWalletTransactionsAsync(
                request.WalletId,
                request.FromDate,
                request.ToDate,
                request.Skip,
                request.Take);
        }
    }
}