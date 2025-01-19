using System;

namespace Unistream.TransactionsApi.V1.Contracts
{
    /// <summary>
    /// Результат операции отаката транзакции
    /// </summary>
    public class RevertTransactionOperationResultModel
    {
        /// <summary>
        /// Дата отката транзакции
        /// </summary>
        public DateTimeOffset RevertDateTime { get; set; }

        /// <summary>
        /// Баланс клиента
        /// </summary>
        public decimal ClientBalance { get; set; }
    }
}
