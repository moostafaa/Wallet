using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WalletService.Application.Commands.WalletAccount;
using WalletService.Application.Models.DTOs;
using WalletService.Application.Queries.WalletAccount;

namespace WalletService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WalletAccountController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public WalletAccountController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        
        [HttpPost("register")]
        public async Task<ActionResult<Guid>> RegisterAccount([FromBody] CreateWalletAccountCommand command)
        {
            var accountId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetWalletAccount), new { id = accountId }, accountId);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<WalletAccountDto>> GetWalletAccount(Guid id)
        {
            var query = new GetWalletAccountQuery { AccountId = id };
            var account = await _mediator.Send(query);
            
            if (account == null)
                return NotFound();
                
            return account;
        }
        
        [HttpPut("{id}/kyc-level")]
        public async Task<ActionResult> UpdateKycLevel(Guid id, [FromBody] UpdateKycLevelCommand command)
        {
            if (id != command.AccountId)
                return BadRequest("Account ID mismatch");
                
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}