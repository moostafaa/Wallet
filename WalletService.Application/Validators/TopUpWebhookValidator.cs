using FluentValidation;
using WalletService.Application.Models.Webhooks;

namespace WalletService.Application.Validators
{
    public class TopUpWebhookValidator : AbstractValidator<TopUpWebhookPayload>
    {
        public TopUpWebhookValidator()
        {
            RuleFor(x => x.RequestId)
                .NotEmpty();
                
            RuleFor(x => x.TransactionReference)
                .NotEmpty()
                .MaximumLength(100);
                
            RuleFor(x => x.Status)
                .NotEmpty()
                .Must(x => x is "completed" or "failed")
                .WithMessage("Status must be either 'completed' or 'failed'");
                
            RuleFor(x => x.Currency)
                .NotEmpty()
                .Length(3)
                .Must(x => x is "USD" or "EUR" or "GBP")
                .WithMessage("Currency must be USD, EUR, or GBP");
                
            RuleFor(x => x.Amount)
                .GreaterThan(0);
                
            RuleFor(x => x.PaymentMethod)
                .NotEmpty()
                .MaximumLength(50);
                
            RuleFor(x => x.ProcessedAt)
                .NotEmpty();
                
            When(x => x.Status == "failed", () =>
            {
                RuleFor(x => x.Error)
                    .NotNull()
                    .WithMessage("Error details are required when status is 'failed'");
                    
                RuleFor(x => x.Error.Code)
                    .NotEmpty()
                    .When(x => x.Error != null);
                    
                RuleFor(x => x.Error.Message)
                    .NotEmpty()
                    .When(x => x.Error != null);
            });
        }
    }
}