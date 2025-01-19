using System;

namespace Unistream.TransactionsApi.V1.Contracts
{
    /// <summary>
    /// Результат операции транзакции
    /// </summary>
    public class InsertTransactionOperationResultModel
    {
        /// <summary>
        /// Дата операции
        /// </summary>
        public DateTimeOffset InsertDateTime { get; set; }

        /// <summary>
        /// Баланс клиента
        /// </summary>
        public decimal ClientBalance { get; set; }
    }
}
