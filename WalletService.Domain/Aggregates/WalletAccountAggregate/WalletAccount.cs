using System;
using System.Collections.Generic;
using WalletService.Domain.Events;

namespace WalletService.Domain.Aggregates.WalletAccountAggregate
{
    public class WalletAccount
    {
        private readonly List<Guid> _walletIds = new List<Guid>();
        
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public AccountType Type { get; private set; }
        public AccountStatus Status { get; private set; }
        public KycLevel KycLevel { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? ClosedAt { get; private set; }
        public decimal DailyTransactionLimit { get; private set; }
        public decimal MonthlyTransactionLimit { get; private set; }
        public string KycDocumentReference { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public string PrimaryCurrency { get; private set; }
        
        public IReadOnlyCollection<Guid> WalletIds => _walletIds.AsReadOnly();
        
        private WalletAccount() { }
        
        public WalletAccount(
            Guid id, 
            string name, 
            AccountType type, 
            string email, 
            string phoneNumber,
            string primaryCurrency)
        {
            Id = id;
            Name = name;
            Type = type;
            Email = email;
            PhoneNumber = phoneNumber;
            PrimaryCurrency = primaryCurrency;
            Status = AccountStatus.Active;
            KycLevel = KycLevel.None;
            CreatedAt = DateTime.UtcNow;
            SetTransactionLimits();
            
            AddDomainEvent(new WalletAccountCreatedEvent(
                Id, 
                Name, 
                Type.ToString(), 
                KycLevel.ToString(),
                Email,
                PhoneNumber,
                PrimaryCurrency));
        }
        
        public void AddWallet(Guid walletId)
        {
            if (Status != AccountStatus.Active)
                throw new InvalidOperationException("Cannot add wallet to inactive account");
                
            if (_walletIds.Contains(walletId))
                throw new InvalidOperationException("Wallet already added to account");
                
            _walletIds.Add(walletId);
            
            AddDomainEvent(new WalletAddedToAccountEvent(Id, walletId));
        }
        
        public void UpdateKycLevel(KycLevel level, string documentReference)
        {
            if (level < KycLevel)
                throw new InvalidOperationException("Cannot downgrade KYC level");
                
            var oldLevel = KycLevel;
            KycLevel = level;
            KycDocumentReference = documentReference;
            SetTransactionLimits();
            
            AddDomainEvent(new KycLevelChangedEvent(Id, oldLevel.ToString(), level.ToString(), documentReference));
        }
        
        private void SetTransactionLimits()
        {
            switch (KycLevel)
            {
                case KycLevel.None:
                    DailyTransactionLimit = 1000;
                    MonthlyTransactionLimit = 5000;
                    break;
                case KycLevel.Basic:
                    DailyTransactionLimit = 5000;
                    MonthlyTransactionLimit = 20000;
                    break;
                case KycLevel.Standard:
                    DailyTransactionLimit = 10000;
                    MonthlyTransactionLimit = 50000;
                    break;
                case KycLevel.Advanced:
                    DailyTransactionLimit = 50000;
                    MonthlyTransactionLimit = 200000;
                    break;
            }
            
            AddDomainEvent(new TransactionLimitsUpdatedEvent(Id, DailyTransactionLimit, MonthlyTransactionLimit));
        }
        
        public void Close()
        {
            if (Status == AccountStatus.Closed)
                throw new InvalidOperationException("Account already closed");
                
            Status = AccountStatus.Closed;
            ClosedAt = DateTime.UtcNow;
            
            AddDomainEvent(new WalletAccountClosedEvent(Id));
        }
        
        public void Suspend(string reason)
        {
            if (Status != AccountStatus.Active)
                throw new InvalidOperationException("Cannot suspend non-active account");
                
            Status = AccountStatus.Suspended;
            
            AddDomainEvent(new WalletAccountSuspendedEvent(Id, reason));
        }
        
        public void Reactivate()
        {
            if (Status != AccountStatus.Suspended)
                throw new InvalidOperationException("Can only reactivate suspended accounts");
                
            Status = AccountStatus.Active;
            
            AddDomainEvent(new WalletAccountReactivatedEvent(Id));
        }
        
        public void ApplyEvent(WalletAccountCreatedEvent @event)
        {
            Id = @event.AccountId;
            Name = @event.Name;
            Type = Enum.Parse<AccountType>(@event.Type);
            Status = AccountStatus.Active;
            KycLevel = Enum.Parse<KycLevel>(@event.KycLevel);
            Email = @event.Email;
            PhoneNumber = @event.PhoneNumber;
            PrimaryCurrency = @event.PrimaryCurrency;
            CreatedAt = @event.Timestamp;
            SetTransactionLimits();
        }
        
        public void ApplyEvent(WalletAddedToAccountEvent @event)
        {
            if (!_walletIds.Contains(@event.WalletId))
                _walletIds.Add(@event.WalletId);
        }
        
        public void ApplyEvent(KycLevelChangedEvent @event)
        {
            KycLevel = Enum.Parse<KycLevel>(@event.NewLevel);
            KycDocumentReference = @event.DocumentReference;
            SetTransactionLimits();
        }
        
        public void ApplyEvent(WalletAccountClosedEvent @event)
        {
            Status = AccountStatus.Closed;
            ClosedAt = @event.Timestamp;
        }
        
        public void ApplyEvent(WalletAccountSuspendedEvent @event)
        {
            Status = AccountStatus.Suspended;
        }
        
        public void ApplyEvent(WalletAccountReactivatedEvent @event)
        {
            Status = AccountStatus.Active;
        }
        
        private List<object> _domainEvents = new List<object>();
        public IReadOnlyCollection<object> DomainEvents => _domainEvents.AsReadOnly();
        
        public void AddDomainEvent(object eventItem)
        {
            _domainEvents.Add(eventItem);
        }
        
        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
    
    public enum AccountType
    {
        User,
        Merchant
    }
    
    public enum AccountStatus
    {
        Active,
        Suspended,
        Closed
    }
    
    public enum KycLevel
    {
        None,
        Basic,
        Standard,
        Advanced
    }
}