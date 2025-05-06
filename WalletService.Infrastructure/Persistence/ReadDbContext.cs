```csharp
// Update ReadDbContext.cs to include new models
public class ReadDbContext : DbContext
{
    public DbSet<WithdrawalRequestReadModel> WithdrawalRequests { get; set; }
    public DbSet<SettlementBatchReadModel> SettlementBatches { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Existing configuration...
        
        modelBuilder.Entity<WithdrawalRequestReadModel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.BankAccountNumber).HasMaxLength(50);
            entity.Property(e => e.BankRoutingNumber).HasMaxLength(20);
            entity.Property(e => e.BankName).HasMaxLength(100);
            entity.Property(e => e.AccountHolderName).HasMaxLength(100);
            entity.Property(e => e.ProcessedBy).HasMaxLength(100);
        });
        
        modelBuilder.Entity<SettlementBatchReadModel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.BankAccountNumber).HasMaxLength(50);
            entity.Property(e => e.BankRoutingNumber).HasMaxLength(20);
            entity.Property(e => e.BankName).HasMaxLength(100);
            entity.Property(e => e.AccountHolderName).HasMaxLength(100);
            entity.Property(e => e.Reference).HasMaxLength(100);
            
            entity.HasMany(e => e.Transactions)
                .WithOne()
                .HasForeignKey(e => e.BatchId);
        });
    }
}

public class WithdrawalRequestReadModel
{
    public Guid Id { get; set; }
    public Guid WalletId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string BankAccountNumber { get; set; }
    public string BankRoutingNumber { get; set; }
    public string BankName { get; set; }
    public string AccountHolderName { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public string ProcessedBy { get; set; }
    public Guid? TransactionId { get; set; }
}

public class SettlementBatchReadModel
{
    public Guid Id { get; set; }
    public Guid MerchantId { get; set; }
    public string Currency { get; set; }
    public decimal TotalAmount { get; set; }
    public string BankAccountNumber { get; set; }
    public string BankRoutingNumber { get; set; }
    public string BankName { get; set; }
    public string AccountHolderName { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public string Reference { get; set; }
    public ICollection<SettlementTransactionReadModel> Transactions { get; set; }
}

public class SettlementTransactionReadModel
{
    public Guid BatchId { get; set; }
    public Guid TransactionId { get; set; }
    public decimal Amount { get; set; }
}
```