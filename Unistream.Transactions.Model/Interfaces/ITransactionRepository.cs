using Unistream.Clients.Model.Interfaces;
using Unistream.Clients.Model.Models;
using Unistream.Transactions.Model.Models;
using Unistream.Transactions.Model.Models.Enums;

namespace Unistream.Transactions.Model.Interfaces
{
    /// <summary>
    /// Операции с транзакциями БД
    /// </summary>
    public interface ITransactionRepository
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
