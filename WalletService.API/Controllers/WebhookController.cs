using System;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletService.Application.Commands.Wallet;
using WalletService.Application.Models.Webhooks;
using WalletService.Domain.Authorization;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace WalletService.API.Controllers
{
    [ApiController]
    [Route("api/webhooks")]
    [Authorize]
    public class WebhookController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly string _webhookSecret;
        
        public WebhookController(
            IMediator mediator,
            IConfiguration configuration)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _webhookSecret = configuration["Webhook:Secret"] ?? throw new ArgumentException("Webhook secret not configured");
        }
        
        [HttpPost("topup")]
        [Authorize(Policy = nameof(Permission.ApproveTransaction))]
        public async Task<IActionResult> HandleTopUpWebhook([FromBody] TopUpWebhookPayload payload)
        {
            // Get the raw request body for signature validation
            string rawBody;
            using (var reader = new StreamReader(Request.Body))
            {
                Request.Body.Position = 0; // Reset position to start
                rawBody = await reader.ReadToEndAsync();
                Request.Body.Position = 0; // Reset for model binding
            }

            if (!await IsValidWebhookSignature(Request.Headers, rawBody))
            {
                return Unauthorized("Invalid webhook signature");
            }
            
            var command = new CompleteTopUpCommand
            {
                RequestId = payload.RequestId,
                TransactionReference = payload.TransactionReference
            };
            
            await _mediator.Send(command);
            return Ok();
        }
        
        private async Task<bool> IsValidWebhookSignature(IHeaderDictionary headers, string payload)
        {
            // Get webhook signature and timestamp from headers
            if (!headers.TryGetValue("X-Webhook-Signature", out var signature) ||
                !headers.TryGetValue("X-Webhook-Timestamp", out var timestamp))
            {
                return false;
            }

            // Validate timestamp is within acceptable window (5 minutes)
            if (!long.TryParse(timestamp, out var timestampValue))
                return false;
                
            var timestampDate = DateTimeOffset.FromUnixTimeSeconds(timestampValue).UtcDateTime;
            if (DateTime.UtcNow.Subtract(timestampDate).TotalMinutes > 5)
                return false;

            // Create the string to sign
            var stringToSign = $"{timestamp}.{payload}";

            // Calculate HMAC
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_webhookSecret)))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToSign));
                var computedSignature = BitConverter.ToString(hash).Replace("-", "").ToLower();
                
                // Constant-time string comparison to prevent timing attacks
                return CryptographicOperations.FixedTimeEquals(
                    Convert.FromHexString(signature),
                    Convert.FromHexString(computedSignature));
            }
        }
    }
}