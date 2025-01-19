using System.Threading.Tasks;
using System.Threading;
using Unistream.Transactions.Model.Models.Enums;
using Unistream.Transactions.Model.Interfaces;
using Unistream.Transactions.Model.Models;

namespace Unistream.TransactionsApi.Services
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
    }
}
