using System.Threading.Tasks;
using System.Threading;
using Unistream.Transactions.Model.Models.Enums;
using Unistream.Transactions.Model.Interfaces;
using System;
using Unistream.Clients.Model.Models;

namespace Unistream.TransactionsApi.V1.Services
{
    /// <summary>
    /// Интерфейс обработки транзакций
    /// </summary>
    public interface ITransactionProcessingService
    {
        /// <summary>
        /// Обработка транзакции
        /// </summary>
        public Task<ClientActualBalanceModel> ProcessTransactionAsync(ITransaction transaction,
            TransactionOperationType transactionOperationType,
            CancellationToken cancellationToken);

        /// <summary>
        /// Откат транзакции
        /// </summary>
        public Task<ClientActualBalanceModel> RevertTransactionAsync(Guid transactionId,
            CancellationToken cancellationToken);
    }
}
