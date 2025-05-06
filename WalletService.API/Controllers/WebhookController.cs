using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WalletService.Application.Commands.Wallet;
using WalletService.Application.Models.Webhooks;

namespace WalletService.API.Controllers
{
    [ApiController]
    [Route("api/webhooks")]
    public class WebhookController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public WebhookController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        
        [HttpPost("topup")]
        public async Task<IActionResult> HandleTopUpWebhook([FromBody] TopUpWebhookPayload payload)
        {
            if (!IsValidWebhookSignature(Request.Headers))
            {
                return Unauthorized();
            }
            
            var command = new CompleteTopUpCommand
            {
                RequestId = payload.RequestId,
                TransactionReference = payload.TransactionReference
            };
            
            await _mediator.Send(command);
            return Ok();
        }
        
        private bool IsValidWebhookSignature(IHeaderDictionary headers)
        {
            // Get webhook signature from headers
            if (!headers.TryGetValue("X-Webhook-Signature", out var signature))
                return false;
                
            // Get timestamp from headers
            if (!headers.TryGetValue("X-Webhook-Timestamp", out var timestamp))
                return false;
                
            // Validate timestamp is within acceptable window (5 minutes)
            if (!long.TryParse(timestamp, out var timestampValue))
                return false;
                
            var timestampDate = DateTimeOffset.FromUnixTimeSeconds(timestampValue).UtcDateTime;
            if (DateTime.UtcNow.Subtract(timestampDate).TotalMinutes > 5)
                return false;
                
            // TODO: Implement actual signature validation using HMAC
            return true;
        }
    }
}