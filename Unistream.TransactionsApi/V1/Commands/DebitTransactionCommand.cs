using System.Threading.Tasks;
using System.Threading;
using System;
using Unistream.Transactions.Model.Interfaces;
using Serilog;
using Unistream.TransactionsApi.Services;
using Unistream.Transactions.Model.Models.Enums;
using Unistream.TransactionsApi.V1.Contracts;
using Unistream.TransactionsApi.V1.Helpers;
using Unistream.TransactionsApi.ErrorHandling;

namespace Unistream.TransactionsApi.V1.Commands
{
    /// <summary>
    /// Команда списания средств со счёта
    /// </summary>
    public class DebitTransactionCommand : MediatR.IRequest<InsertTransactionOperationResultModel>, ITransaction
    {
        ///<inheritdoc/>
        public Guid Id { get; set; }

        ///<inheritdoc/>
        public Guid ClientId { get; set; }

        ///<inheritdoc/>
        public DateTimeOffset DateTime { get; set; }

        ///<inheritdoc/>
        public decimal Amount { get; set; }

        /// <summary>
        /// Зачисление средств на счёт
        /// </summary>
        public class DebitTransactionCommandHandler : MediatR.IRequestHandler<DebitTransactionCommand, InsertTransactionOperationResultModel>
        {
            /// <summary>
            /// Сервис транзакций
            /// </summary>
            private readonly ITransactionProcessingService _transactionService;

            /// <summary>
            /// Конструктор
            /// </summary>
            public DebitTransactionCommandHandler(ITransactionProcessingService transactionService)
            {
                _transactionService = transactionService;
            }

            ///<inheritdoc/>
            public async Task<InsertTransactionOperationResultModel> Handle(DebitTransactionCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var clientBalance = await _transactionService.ProcessTransactionAsync(request, TransactionOperationType.Debit, cancellationToken);

                    return ContractsMapper.MapInsertTransaction(clientBalance);
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(ex, "Ошибка при начислении средств на счёт клиенту с Id {0}", request.ClientId);
                    throw ErrorFactory.Create(ErrorCode.UnspecifiedError, ex.Message);
                }
            }
        }
    }
}
