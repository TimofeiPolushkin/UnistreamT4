using Unistream.Transactions.Model.Models;

namespace Unistream.Transactions.Model.Interfaces
{
    /// <summary>
    /// Операции с транзакциями БД
    /// </summary>
    public interface ITransactionRepository
    {
        /// <summary>
        /// Обработка транзакции зачисления
        /// </summary>
        public Task<ClientActualBalanceModel> CreditTransactionAsync(ITransaction transaction,
                    CancellationToken cancellationToken);

        /// <summary>
        /// Обработка транзакции списания
        /// </summary>
        public Task<ClientActualBalanceModel> DebitTransactionAsync(ITransaction transaction,
                    CancellationToken cancellationToken);
    }
}
