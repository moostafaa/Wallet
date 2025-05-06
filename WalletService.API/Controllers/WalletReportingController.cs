using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WalletService.Application.Models.DTOs;
using WalletService.Application.Queries.Wallet;

namespace WalletService.API.Controllers
{
    [ApiController]
    [Route("api/wallet-reporting")]
    public class WalletReportingController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public WalletReportingController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        
        [HttpGet("account/{accountId}/summary")]
        public async Task<ActionResult<AccountSummaryDto>> GetAccountSummary(Guid accountId)
        {
            var query = new GetAccountSummaryQuery { AccountId = accountId };
            var summary = await _mediator.Send(query);
            
            if (summary == null)
                return NotFound();
                
            return summary;
        }
        
        [HttpGet("wallet/{walletId}/summary")]
        public async Task<ActionResult<WalletSummaryDto>> GetWalletSummary(Guid walletId)
        {
            var query = new GetWalletSummaryQuery { WalletId = walletId };
            var summary = await _mediator.Send(query);
            
            if (summary == null)
                return NotFound();
                
            return summary;
        }
        
        [HttpGet("wallet/{walletId}/transactions")]
        public async Task<ActionResult<TransactionHistoryDto>> GetTransactionHistory(
            Guid walletId,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] string transactionType,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var query = new GetTransactionHistoryQuery
            {
                WalletId = walletId,
                FromDate = fromDate,
                ToDate = toDate,
                TransactionType = transactionType,
                Page = page,
                PageSize = pageSize
            };
            
            var history = await _mediator.Send(query);
            return history;
        }
    }
}