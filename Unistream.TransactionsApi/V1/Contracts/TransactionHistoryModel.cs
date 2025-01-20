using System;

namespace Unistream.TransactionsApi.V1.Contracts
{
    public class TransactionHistoryModel
    {
        ///<inheritdoc/>
        public Guid UniId { get; set; }

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
