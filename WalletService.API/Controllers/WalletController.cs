using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletService.Application.Commands.Wallet;
using WalletService.Application.Models.DTOs;
using WalletService.Application.Queries.Wallet;
using WalletService.Domain.Authorization;

namespace WalletService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WalletController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public WalletController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        
        [HttpPost]
        [Authorize(Policy = nameof(Permission.CreateWallet))]
        public async Task<ActionResult<Guid>> CreateWallet([FromBody] CreateWalletCommand command)
        {
            var walletId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetWallet), new { id = walletId }, walletId);
        }
        
        [HttpGet("{id}")]
        [Authorize(Policy = nameof(Permission.ViewWallet))]
        public async Task<ActionResult<WalletDto>> GetWallet(Guid id)
        {
            var query = new GetWalletQuery { WalletId = id };
            var wallet = await _mediator.Send(query);
            
            if (wallet == null)
                return NotFound();
                
            return wallet;
        }
        
        [HttpGet("account/{accountId}")]
        [Authorize(Policy = nameof(Permission.ViewWallet))]
        public async Task<ActionResult<IEnumerable<WalletDto>>> GetAccountWallets(Guid accountId)
        {
            var query = new GetAccountWalletsQuery { AccountId = accountId };
            var wallets = await _mediator.Send(query);
            return Ok(wallets);
        }
        
        [HttpGet("{id}/transactions")]
        [Authorize(Policy = nameof(Permission.ViewTransactions))]
        public async Task<ActionResult<IEnumerable<TransactionDto>>> GetWalletTransactions(
            Guid id,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] int skip = 0,
            [FromQuery] int take = 10)
        {
            var query = new GetWalletTransactionsQuery
            {
                WalletId = id,
                FromDate = fromDate,
                ToDate = toDate,
                Skip = skip,
                Take = take
            };
            
            var transactions = await _mediator.Send(query);
            return Ok(transactions);
        }
        
        [HttpPost("add-funds")]
        [Authorize(Policy = nameof(Permission.CreateTransaction))]
        public async Task<ActionResult> AddFunds([FromBody] AddFundsCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        
        [HttpPost("transfer")]
        [Authorize(Policy = nameof(Permission.CreateTransaction))]
        public async Task<ActionResult> TransferFunds([FromBody] TransferFundsCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        
        [HttpPost("withdraw")]
        [Authorize(Policy = nameof(Permission.CreateTransaction))]
        public async Task<ActionResult> WithdrawFunds([FromBody] WithdrawFundsCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        
        [HttpPost("{id}/deactivate")]
        [Authorize(Policy = nameof(Permission.DeactivateWallet))]
        public async Task<ActionResult> DeactivateWallet(Guid id)
        {
            var command = new DeactivateWalletCommand { WalletId = id };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        
        [HttpPost("top-up")]
        [Authorize(Policy = nameof(Permission.CreateTransaction))]
        public async Task<ActionResult<Guid>> InitiateTopUp([FromBody] InitiateTopUpCommand command)
        {
            var requestId = await _mediator.Send(command);
            return Ok(requestId);
        }
        
        [HttpPost("top-up/{requestId}/complete")]
        [Authorize(Policy = nameof(Permission.ApproveTransaction))]
        public async Task<ActionResult> CompleteTopUp(Guid requestId, [FromBody] CompleteTopUpCommand command)
        {
            if (requestId != command.RequestId)
                return BadRequest("Request ID mismatch");
                
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        
        [HttpPost("auto-top-up")]
        [Authorize(Policy = nameof(Permission.CreateTransaction))]
        public async Task<ActionResult<Guid>> CreateAutoTopUpRule([FromBody] CreateAutoTopUpRuleCommand command)
        {
            var ruleId = await _mediator.Send(command);
            return Ok(ruleId);
        }
    }
}