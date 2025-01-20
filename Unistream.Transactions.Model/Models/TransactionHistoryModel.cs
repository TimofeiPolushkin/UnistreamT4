using Unistream.Transactions.Model.EF.Enums;

namespace Unistream.Transactions.Model.Models
{
    public class TransactionHistoryModel
    {
        ///<inheritdoc/>
        public Guid UniId { get; set; }

        ///<inheritdoc/>
        public DateTimeOffset? Created {  get; set; }

        ///<inheritdoc/>
        public DateTimeOffset? Updated { get; set; }

        ///<inheritdoc/>
        public DateTimeOffset? Deleted { get; set; }

        /// <summary>
        /// Тип транзакции
        /// </summary>
        public TransactionHistoryType TransactionType { get; set; }

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
