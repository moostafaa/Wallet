using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WalletService.Application.Commands.Admin;
using WalletService.Application.Models.DTOs;
using WalletService.Application.Queries.Admin;

namespace WalletService.API.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public AdminController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        
        [HttpPost("fee-rules")]
        public async Task<ActionResult<Guid>> CreateFeeRule([FromBody] CreateFeeRuleCommand command)
        {
            var ruleId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetFeeRule), new { id = ruleId }, ruleId);
        }
        
        [HttpGet("fee-rules/{id}")]
        public async Task<ActionResult<FeeRuleDto>> GetFeeRule(Guid id)
        {
            var query = new GetFeeRuleQuery { RuleId = id };
            var rule = await _mediator.Send(query);
            
            if (rule == null)
                return NotFound();
                
            return rule;
        }
        
        [HttpPost("fee-rules/{id}/deactivate")]
        public async Task<ActionResult> DeactivateFeeRule(Guid id)
        {
            var command = new DeactivateFeeRuleCommand { RuleId = id };
            await _mediator.Send(command);
            return Ok();
        }
        
        [HttpPost("exchange-rates")]
        public async Task<ActionResult<Guid>> CreateExchangeRate([FromBody] CreateExchangeRateCommand command)
        {
            var rateId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetExchangeRate), new { id = rateId }, rateId);
        }
        
        [HttpGet("exchange-rates/{id}")]
        public async Task<ActionResult<ExchangeRateDto>> GetExchangeRate(Guid id)
        {
            var query = new GetExchangeRateQuery { RateId = id };
            var rate = await _mediator.Send(query);
            
            if (rate == null)
                return NotFound();
                
            return rate;
        }
        
        [HttpPut("exchange-rates/{id}")]
        public async Task<ActionResult> UpdateExchangeRate(Guid id, [FromBody] UpdateExchangeRateCommand command)
        {
            if (id != command.RateId)
                return BadRequest("Rate ID mismatch");
                
            await _mediator.Send(command);
            return Ok();
        }
        
        [HttpPost("wallets/{id}/block")]
        public async Task<ActionResult> BlockWallet(Guid id, [FromBody] BlockWalletCommand command)
        {
            if (id != command.WalletId)
                return BadRequest("Wallet ID mismatch");
                
            await _mediator.Send(command);
            return Ok();
        }
        
        [HttpPost("wallets/{id}/unblock")]
        public async Task<ActionResult> UnblockWallet(Guid id, [FromBody] UnblockWalletCommand command)
        {
            if (id != command.WalletId)
                return BadRequest("Wallet ID mismatch");
                
            await _mediator.Send(command);
            return Ok();
        }
    }
}