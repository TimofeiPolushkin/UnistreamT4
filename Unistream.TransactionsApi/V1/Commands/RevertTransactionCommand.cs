using System;
using System.Threading;
using System.Threading.Tasks;
using Unistream.Transactions.Model.Interfaces;
using Unistream.TransactionsApi.V1.Contracts;

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

        public class RevertTransactionCommandHandler : MediatR.IRequestHandler<RevertTransactionCommand, RevertTransactionOperationResultModel>
        {
            /// <summary>
            /// Репозиторий транзакций
            /// </summary>
            private readonly ITransactionRepository _transactionRepository;

            /// <summary>
            /// Конструктор
            /// </summary>
            public RevertTransactionCommandHandler(
                ITransactionRepository transactionRepository)
            {
                _transactionRepository = transactionRepository;
            }

            ///<inheritdoc/>
            public async Task<RevertTransactionOperationResultModel> Handle(RevertTransactionCommand request, CancellationToken token)
            {
                try
                {

                }
                catch (Exception ex)
                {
                    //Log.Error(ex, "Ошибка при изменении объекта МБ");
                    throw;
                }

                return null;
            }
        }
    }
}