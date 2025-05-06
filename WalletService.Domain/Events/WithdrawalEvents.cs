```csharp
using System;

namespace WalletService.Domain.Events
{
    public class WithdrawalRequestCreatedEvent : DomainEvent
    {
        public Guid RequestId { get; }
        public Guid WalletId { get; }
        public decimal Amount { get; }
        public string CurrencyCode { get; }
        public string BankAccountNumber { get; }
        public string BankRoutingNumber { get; }
        public string BankName { get; }
        public string AccountHolderName { get; }
        
        public WithdrawalRequestCreatedEvent(
            Guid requestId,
            Guid walletId,
            decimal amount,
            string currencyCode,
            string bankAccountNumber,
            string bankRoutingNumber,
            string bankName,
            string accountHolderName)
        {
            RequestId = requestId;
            WalletId = walletId;
            Amount = amount;
            CurrencyCode = currencyCode;
            BankAccountNumber = bankAccountNumber;
            BankRoutingNumber = bankRoutingNumber;
            BankName = bankName;
            AccountHolderName = accountHolderName;
        }
    }
    
    public class WithdrawalRequestApprovedEvent : DomainEvent
    {
        public Guid RequestId { get; }
        public Guid TransactionId { get; }
        public string ApprovedBy { get; }
        
        public WithdrawalRequestApprovedEvent(
            Guid requestId,
            Guid transactionId,
            string approvedBy)
        {
            RequestId = requestId;
            TransactionId = transactionId;
            ApprovedBy = approvedBy;
        }
    }
    
    public class WithdrawalRequestRejectedEvent : DomainEvent
    {
        public Guid RequestId { get; }
        public string RejectedBy { get; }
        public string Reason { get; }
        
        public WithdrawalRequestRejectedEvent(
            Guid requestId,
            string rejectedBy,
            string reason)
        {
            RequestId = requestId;
            RejectedBy = rejectedBy;
            Reason = reason;
        }
    }
}
```