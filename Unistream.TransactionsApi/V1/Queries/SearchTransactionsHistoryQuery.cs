using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unistream.Transactions.Model.Interfaces;
using Unistream.TransactionsApi.ErrorHandling;
using Unistream.TransactionsApi.V1.Contracts;
using Unistream.TransactionsApi.V1.Helpers;

namespace Unistream.TransactionsApi.V1.Queries
{
    /// <summary>
    /// Получение списка истории транзакций
    /// </summary>
    public class SearchTransactionsHistoryQuery : MediatR.IRequest<List<TransactionHistoryModel>>
    {
        /// <summary>
        /// Параметр пропуска
        /// </summary>
        public int Skip { get; set; }

        /// <summary>
        /// Параметр выборки
        /// </summary>
        public int Take { get; set; } = 1000;

        /// <summary>
        /// Запрос
        /// </summary>
        public class SearchTransactionsHistoryQueryHandler : MediatR.IRequestHandler<SearchTransactionsHistoryQuery, List<TransactionHistoryModel>>
        {
            /// <summary>
            /// Репозиторий транзакций
            /// </summary>
            private readonly ITransactionRepository _transactionRepository;

            /// <summary>
            /// Конструктор
            /// </summary>
            public SearchTransactionsHistoryQueryHandler(
                ITransactionRepository transactionRepository)
            {
                _transactionRepository = transactionRepository;
            }

            ///<inheritdoc/>
            public async Task<List<TransactionHistoryModel>> Handle(SearchTransactionsHistoryQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var transactions = await _transactionRepository.SearchClientsAsync(request.Skip, request.Take, cancellationToken);

                    return transactions
                        .Select(ContractsMapper.Map)
                        .ToList();
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(ex, string.Format("Ошибка запроса списка транзакций}"));
                    throw ErrorFactory.Create(ErrorCode.UnspecifiedError, ex.Message);
                }
            }
        }
    }
}