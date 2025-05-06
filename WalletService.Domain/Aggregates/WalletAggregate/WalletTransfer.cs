using System;

namespace WalletService.Domain.Aggregates.WalletAggregate
{
    public class WalletTransfer
    {
        public Guid Id { get; private set; }
        public Guid SourceWalletId { get; private set; }
        public Guid DestinationWalletId { get; private set; }
        public Guid SourceTransactionId { get; private set; }
        public Guid DestinationTransactionId { get; private set; }
        public string Reference { get; private set; }
        public DateTime CreatedAt { get; private set; }
        
        // For EF Core
        private WalletTransfer() { }
        
        public WalletTransfer(
            Guid id,
            Guid sourceWalletId,
            Guid destinationWalletId,
            Guid sourceTransactionId,
            Guid destinationTransactionId,
            string reference)
        {
            Id = id;
            SourceWalletId = sourceWalletId;
            DestinationWalletId = destinationWalletId;
            SourceTransactionId = sourceTransactionId;
            DestinationTransactionId = destinationTransactionId;
            Reference = reference;
            CreatedAt = DateTime.UtcNow;
        }
    }
}