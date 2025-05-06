using System;

namespace WalletService.Domain.Exceptions
{
    public class WalletInactiveException : Exception
    {
        public Guid WalletId { get; }
        
        public WalletInactiveException(Guid walletId)
            : base($"Wallet {walletId} is inactive")
        {
            WalletId = walletId;
        }
    }
    
    public class CurrencyMismatchException : Exception
    {
        public string ExpectedCurrency { get; }
        public string ActualCurrency { get; }
        
        public CurrencyMismatchException(string actualCurrency, string expectedCurrency)
            : base($"Currency mismatch: expected {expectedCurrency}, but got {actualCurrency}")
        {
            ExpectedCurrency = expectedCurrency;
            ActualCurrency = actualCurrency;
        }
    }
    
    public class InvalidAmountException : Exception
    {
        public InvalidAmountException(string message) : base(message)
        {
        }
    }
    
    public class InsufficientFundsException : Exception
    {
        public Guid WalletId { get; }
        public decimal RequestedAmount { get; }
        public decimal AvailableBalance { get; }
        
        public InsufficientFundsException(Guid walletId, decimal requestedAmount, decimal availableBalance)
            : base($"Insufficient funds in wallet {walletId}: requested {requestedAmount}, available {availableBalance}")
        {
            WalletId = walletId;
            RequestedAmount = requestedAmount;
            AvailableBalance = availableBalance;
        }
    }
    
    public class UnknownCurrencyException : Exception
    {
        public string CurrencyCode { get; }
        
        public UnknownCurrencyException(string currencyCode)
            : base($"Unknown currency code: {currencyCode}")
        {
            CurrencyCode = currencyCode;
        }
    }
}