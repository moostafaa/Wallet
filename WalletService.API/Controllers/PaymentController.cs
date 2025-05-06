using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WalletService.Application.Commands.Wallet;

namespace WalletService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public PaymentController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        
        [HttpPost]
        public async Task<ActionResult<Guid>> InitiatePayment([FromBody] InitiatePaymentCommand command)
        {
            var paymentId = await _mediator.Send(command);
            return Ok(paymentId);
        }
        
        [HttpPost("{id}/complete")]
        public async Task<ActionResult> CompletePayment(Guid id)
        {
            var command = new CompletePaymentCommand { PaymentId = id };
            await _mediator.Send(command);
            return Ok();
        }
    }
}