using System;
using System.Threading;
using System.Threading.Tasks;
using Unistream.Transactions.Model.Interfaces;
using Unistream.Transactions.Model.Models;
using Unistream.Transactions.Model.Models.Enums;

namespace Unistream.TransactionsApi.Services
{
    /// <summary>
    /// Сервис транзакций
    /// </summary>
    public class TransactionProcessingService : ITransactionProcessingService
    {
        /// <summary>
        /// Репозиторий транзакций
        /// </summary>
        private readonly ITransactionRepository _transactionRepository;

        /// <summary>
        /// Конструктор
        /// </summary>
        public TransactionProcessingService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        /// <inheritdoc>/>
        public async Task<ClientActualBalanceModel> ProcessTransactionAsync(ITransaction transaction,
            TransactionOperationType transactionOperationType,
            CancellationToken cancellationToken)
        {
            switch(transactionOperationType)
            {
                case TransactionOperationType.Credit:
                    return await _transactionRepository.CreditTransactionAsync(transaction, cancellationToken);
                case TransactionOperationType.Debit:
                    return await _transactionRepository.DebitTransactionAsync(transaction, cancellationToken);
                default:
                    throw new Exception("Неизвестный тип операции");
            }
        }

        ///// <inheritdoc>/>
        //public async Task<ITransaction> RevertTransactionAsync(Guid transactionId,
        //    CancellationToken cancellationToken)
        //{
            
        //}
    }
}
