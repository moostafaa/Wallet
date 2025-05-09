using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletService.Application.Commands.Risk;
using WalletService.Application.Models.DTOs;
using WalletService.Application.Queries.Risk;
using WalletService.Domain.Authorization;

namespace WalletService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RiskController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public RiskController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        
        [HttpGet("accounts/{accountId}/limits")]
        [Authorize(Policy = nameof(Permission.ViewRiskProfile))]
        public async Task<ActionResult<AccountLimitsDto>> GetAccountLimits(Guid accountId)
        {
            var query = new GetAccountLimitsQuery { AccountId = accountId };
            var limits = await _mediator.Send(query);
            
            if (limits == null)
                return NotFound();
                
            return limits;
        }
        
        [HttpPost("accounts/{accountId}/risk-level")]
        [Authorize(Policy = nameof(Permission.UpdateRiskLevel))]
        public async Task<ActionResult> UpdateRiskLevel(Guid accountId, [FromBody] UpdateRiskLevelCommand command)
        {
            if (accountId != command.AccountId)
                return BadRequest("Account ID mismatch");
                
            await _mediator.Send(command);
            return Ok();
        }
        
        [HttpPost("accounts/{accountId}/freeze")]
        [Authorize(Policy = nameof(Permission.FreezeAccount))]
        public async Task<ActionResult> FreezeAccount(Guid accountId, [FromBody] FreezeAccountCommand command)
        {
            if (accountId != command.AccountId)
                return BadRequest("Account ID mismatch");
                
            await _mediator.Send(command);
            return Ok();
        }
        
        [HttpPost("accounts/{accountId}/unfreeze")]
        [Authorize(Policy = nameof(Permission.UnfreezeAccount))]
        public async Task<ActionResult> UnfreezeAccount(Guid accountId, [FromBody] UnfreezeAccountCommand command)
        {
            if (accountId != command.AccountId)
                return BadRequest("Account ID mismatch");
                
            await _mediator.Send(command);
            return Ok();
        }
    }
}