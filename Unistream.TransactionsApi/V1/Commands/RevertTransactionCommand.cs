using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;
using Unistream.TransactionsApi.ErrorHandling;
using Unistream.TransactionsApi.V1.Contracts;
using Unistream.TransactionsApi.V1.Services;

namespace Unistream.TransactionsApi.V1.Commands
{
    /// <summary>
    /// Команда отката транзакции
    /// </summary>
    public class RevertTransactionCommand : MediatR.IRequest<RevertTransactionOperationResultModel>
    {
        /// <summary>
        /// Идентификатор транзакции
        /// </summary>
        public Guid TransactionId { get; set; }

        /// <summary>
        /// Запрос
        /// </summary>
        public class RevertTransactionCommandHandler : MediatR.IRequestHandler<RevertTransactionCommand, RevertTransactionOperationResultModel>
        {
            /// <summary>
            /// Сервис обрабоки транзакций
            /// </summary>
            private readonly ITransactionProcessingService _transactionProcessingService;

            /// <summary>
            /// Конструктор
            /// </summary>
            public RevertTransactionCommandHandler(
                ITransactionProcessingService transactionProcessingService)
            {
                _transactionProcessingService = transactionProcessingService;
            }

            ///<inheritdoc/>
            public async Task<RevertTransactionOperationResultModel> Handle(RevertTransactionCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    await _transactionProcessingService.RevertTransactionAsync(request.TransactionId, cancellationToken);
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(ex, string.Format("Ошибка отката транзакции Id {0}", request.TransactionId));
                    throw ErrorFactory.Create(ErrorCode.UnspecifiedError, ex.Message);
                }

                return null;
            }
        }
    }
}