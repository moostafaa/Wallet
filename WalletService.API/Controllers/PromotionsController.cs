using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletService.Application.Commands.Promotions;
using WalletService.Application.Models.DTOs;
using WalletService.Application.Queries.Promotions;
using WalletService.Domain.Authorization;

namespace WalletService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PromotionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public PromotionsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        
        [HttpPost("cashback-rules")]
        [Authorize(Policy = nameof(Permission.ManageSettings))]
        public async Task<ActionResult<Guid>> CreateCashbackRule([FromBody] CreateCashbackRuleCommand command)
        {
            var ruleId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetCashbackRule), new { id = ruleId }, ruleId);
        }
        
        [HttpGet("cashback-rules/{id}")]
        [Authorize(Policy = nameof(Permission.ViewWallet))]
        public async Task<ActionResult<CashbackRuleDto>> GetCashbackRule(Guid id)
        {
            var query = new GetCashbackRuleQuery { RuleId = id };
            var rule = await _mediator.Send(query);
            
            if (rule == null)
                return NotFound();
                
            return rule;
        }
        
        [HttpPost("cashback-rules/{id}/deactivate")]
        [Authorize(Policy = nameof(Permission.ManageSettings))]
        public async Task<ActionResult> DeactivateCashbackRule(Guid id)
        {
            var command = new DeactivateCashbackRuleCommand { RuleId = id };
            await _mediator.Send(command);
            return Ok();
        }
        
        [HttpPost("referral-codes")]
        [Authorize(Policy = nameof(Permission.CreateTransaction))]
        public async Task<ActionResult<string>> GenerateReferralCode([FromBody] GenerateReferralCodeCommand command)
        {
            var referralCode = await _mediator.Send(command);
            return Ok(referralCode);
        }
        
        [HttpPost("referral-codes/validate")]
        [Authorize(Policy = nameof(Permission.ViewWallet))]
        public async Task<ActionResult<bool>> ValidateReferralCode([FromBody] ValidateReferralCodeCommand command)
        {
            var isValid = await _mediator.Send(command);
            return Ok(isValid);
        }
        
        [HttpGet("referral-bonuses/{userId}")]
        [Authorize(Policy = nameof(Permission.ViewWallet))]
        public async Task<ActionResult<ReferralSummaryDto>> GetReferralSummary(Guid userId)
        {
            var query = new GetReferralSummaryQuery { UserId = userId };
            var summary = await _mediator.Send(query);
            
            if (summary == null)
                return NotFound();
                
            return summary;
        }
    }
}