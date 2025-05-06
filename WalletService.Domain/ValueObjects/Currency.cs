using System;
using System.Collections.Generic;
using WalletService.Domain.Exceptions;

namespace WalletService.Domain.ValueObjects
{
    public class Currency : IEquatable<Currency>
    {
        private static readonly Dictionary<string, Currency> _currencies = new Dictionary<string, Currency>
        {
            { "USD", new Currency("USD", 2) },
            { "EUR", new Currency("EUR", 2) },
            { "GBP", new Currency("GBP", 2) },
            { "JPY", new Currency("JPY", 0) },
            { "BTC", new Currency("BTC", 8) },
            { "ETH", new Currency("ETH", 18) }
        };
        
        public string Code { get; }
        public int DecimalPlaces { get; }
        
        private Currency(string code, int decimalPlaces)
        {
            Code = code;
            DecimalPlaces = decimalPlaces;
        }
        
        public static Currency FromCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Currency code cannot be empty", nameof(code));
                
            code = code.ToUpperInvariant();
            
            if (!_currencies.TryGetValue(code, out var currency))
                throw new UnknownCurrencyException(code);
                
            return currency;
        }
        
        public static IEnumerable<Currency> GetAllCurrencies() => _currencies.Values;
        
        public bool Equals(Currency other)
        {
            if (other is null)
                return false;
                
            return Code == other.Code;
        }
        
        public override bool Equals(object obj)
        {
            return obj is Currency currency && Equals(currency);
        }
        
        public override int GetHashCode()
        {
            return Code.GetHashCode();
        }
        
        public static bool operator ==(Currency left, Currency right)
        {
            if (left is null)
                return right is null;
                
            return left.Equals(right);
        }
        
        public static bool operator !=(Currency left, Currency right)
        {
            return !(left == right);
        }
        
        public override string ToString()
        {
            return Code;
        }
    }
}