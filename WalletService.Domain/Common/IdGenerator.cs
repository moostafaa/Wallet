using System;
using System.Collections.Generic;

namespace WalletService.Domain.Common
{
    public static class IdGenerator
    {
        private static readonly Dictionary<string, SnowflakeId> _generators = new();
        
        static IdGenerator()
        {
            // Initialize generators for different types
            // Using different worker IDs for each type to ensure uniqueness
            _generators["Wallet"] = new SnowflakeId(1, 1);
            _generators["Account"] = new SnowflakeId(2, 1);
            _generators["Transaction"] = new SnowflakeId(3, 1);
            _generators["Event"] = new SnowflakeId(4, 1);
            _generators["TopUp"] = new SnowflakeId(5, 1);
            _generators["Withdrawal"] = new SnowflakeId(6, 1);
            _generators["Dispute"] = new SnowflakeId(7, 1);
            _generators["Settlement"] = new SnowflakeId(8, 1);
            _generators["FeeRule"] = new SnowflakeId(9, 1);
            _generators["CashbackRule"] = new SnowflakeId(10, 1);
            _generators["ReferralBonus"] = new SnowflakeId(11, 1);
            _generators["AutoTopUp"] = new SnowflakeId(12, 1);
            _generators["MoneyRequest"] = new SnowflakeId(13, 1);
            _generators["RecurringTransfer"] = new SnowflakeId(14, 1);
            _generators["RiskProfile"] = new SnowflakeId(15, 1);
        }
        
        public static long NewId(string type)
        {
            if (!_generators.ContainsKey(type))
                throw new ArgumentException($"No ID generator configured for type: {type}");
                
            return _generators[type].NextId();
        }
        
        public static long NewEventId()
        {
            return _generators["Event"].NextId();
        }
    }
}