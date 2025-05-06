using System;
using WalletService.Domain.Exceptions;

namespace WalletService.Domain.ValueObjects
{
    public class Money : IEquatable<Money>
    {
        public decimal Amount { get; }
        public Currency Currency { get; }
        
        public Money(decimal amount, Currency currency)
        {
            Amount = amount;
            Currency = currency ?? throw new ArgumentNullException(nameof(currency));
        }
        
        public static Money Zero(Currency currency) => new Money(0, currency);
        
        public Money Add(Money money)
        {
            if (money.Currency != Currency)
                throw new CurrencyMismatchException(money.Currency.Code, Currency.Code);
                
            return new Money(Amount + money.Amount, Currency);
        }
        
        public Money Subtract(Money money)
        {
            if (money.Currency != Currency)
                throw new CurrencyMismatchException(money.Currency.Code, Currency.Code);
                
            return new Money(Amount - money.Amount, Currency);
        }
        
        public Money Negate() => new Money(-Amount, Currency);
        
        public Money Multiply(decimal factor) => new Money(Amount * factor, Currency);
        
        public bool Equals(Money other)
        {
            if (other is null)
                return false;
                
            return Amount == other.Amount && Currency.Equals(other.Currency);
        }
        
        public override bool Equals(object obj)
        {
            return obj is Money money && Equals(money);
        }
        
        public override int GetHashCode()
        {
            return HashCode.Combine(Amount, Currency);
        }
        
        public static bool operator ==(Money left, Money right)
        {
            if (left is null)
                return right is null;
                
            return left.Equals(right);
        }
        
        public static bool operator !=(Money left, Money right)
        {
            return !(left == right);
        }
        
        public override string ToString()
        {
            return $"{Amount.ToString("0.00")} {Currency.Code}";
        }
    }
}