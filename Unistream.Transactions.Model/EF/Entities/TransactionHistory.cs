namespace Unistream.Transactions.Model.EF.Entities
{
    public class TransactionHistory : ITransactionEntity
    {
        ///<inheritdoc/>
        public long Id { get; set; }

        ///<inheritdoc/>
        public Guid UniId { get; set; }

        ///<inheritdoc/>
        public DateTimeOffset? Created {  get; set; }

        ///<inheritdoc/>
        public DateTimeOffset? Updated { get; set; }

        ///<inheritdoc/>
        public DateTimeOffset? Deleted { get; set; }

        ///<inheritdoc/>
        public byte[] Version { get; set; }

        /// <summary>
        /// Тип транзакции
        /// </summary>
        public Enums.TransactionHistoryType TransactionType { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Идентификатор клиента
        /// </summary>
        public Guid ClientId { get; set; }

        /// <summary>
        /// Дата и время транзакции
        /// </summary>
        public DateTimeOffset TransactionDateTime { get; set; }

        /// <summary>
        /// Признак отката транзакции
        /// </summary>
        public bool? IsRollback { get; set; }

        /// <summary>
        /// Признак отката транзакции
        /// </summary>
        public DateTimeOffset? RollbackDateTime { get; set; }
    }
}
