using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unistream.Clients.Model.Interfaces;
using Unistream.Clients.Model.Models;
using Unistream.Transactions.Model.Interfaces;
using Unistream.Transactions.Model.Models.Enums;
using Unistream.TransactionsApi.ErrorHandling;
using Unistream.TransactionsApi.V1.Helpers;

namespace Unistream.TransactionsApi.V1.Services
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
        /// Репозиторий клиентов
        /// </summary>
        private readonly IClientRepository _clientRepository;

        /// <summary>
        /// Конструктор
        /// </summary>
        public TransactionProcessingService(ITransactionRepository transactionRepository,
            IClientRepository clientRepository)
        {
            _transactionRepository = transactionRepository;
            _clientRepository = clientRepository;
        }

        /// <inheritdoc>/>
        public async Task<ClientActualBalanceModel> ProcessTransactionAsync(ITransaction transaction,
            TransactionOperationType transactionOperationType,
            CancellationToken cancellationToken)
        {
            var client = await _clientRepository.GetClientByIdAsync(transaction.ClientId, cancellationToken);

            var validateResult = TransactionValidator.TransactionValidate(transaction, client, transactionOperationType);

            if (validateResult != null && validateResult.Errors.Any())
            {
                string errorMessage = string.Join("\n", validateResult.Errors);
                throw ErrorFactory.Create(ErrorCode.WrongRequest, errorMessage);
            }

            return await _transactionRepository.ProcessTransactionAsync(transaction, transactionOperationType, cancellationToken);
        }

        /// <inheritdoc>/>
        public async Task<ClientActualBalanceModel> RevertTransactionAsync(Guid transactionId,
            CancellationToken cancellationToken)
        {
            return await _transactionRepository.RevertTransactionAsync(transactionId, cancellationToken);
        }
    }
}
