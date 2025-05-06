using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WalletService.Application.Commands.Dispute;
using WalletService.Application.Models.DTOs;
using WalletService.Application.Queries.Dispute;

namespace WalletService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DisputeController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public DisputeController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateDispute([FromBody] CreateDisputeCommand command)
        {
            var disputeId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetDispute), new { id = disputeId }, disputeId);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<DisputeDto>> GetDispute(Guid id)
        {
            var query = new GetDisputeQuery { DisputeId = id };
            var dispute = await _mediator.Send(query);
            
            if (dispute == null)
                return NotFound();
                
            return dispute;
        }
        
        [HttpPost("{id}/approve")]
        public async Task<ActionResult> ApproveDispute(Guid id, [FromBody] ApproveDisputeCommand command)
        {
            if (id != command.DisputeId)
                return BadRequest("Dispute ID mismatch");
                
            await _mediator.Send(command);
            return Ok();
        }
        
        [HttpPost("{id}/reject")]
        public async Task<ActionResult> RejectDispute(Guid id, [FromBody] RejectDisputeCommand command)
        {
            if (id != command.DisputeId)
                return BadRequest("Dispute ID mismatch");
                
            await _mediator.Send(command);
            return Ok();
        }
        
        [HttpPost("{id}/escalate")]
        public async Task<ActionResult> EscalateDispute(Guid id, [FromBody] EscalateDisputeCommand command)
        {
            if (id != command.DisputeId)
                return BadRequest("Dispute ID mismatch");
                
            await _mediator.Send(command);
            return Ok();
        }
        
        [HttpPost("refund")]
        public async Task<ActionResult<Guid>> IssueManualRefund([FromBody] IssueManualRefundCommand command)
        {
            var refundId = await _mediator.Send(command);
            return Ok(refundId);
        }
    }
}