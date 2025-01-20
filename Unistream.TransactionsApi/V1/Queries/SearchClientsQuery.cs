using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unistream.Clients.Model.Interfaces;
using Unistream.TransactionsApi.ErrorHandling;
using Unistream.TransactionsApi.V1.Contracts;
using Unistream.TransactionsApi.V1.Helpers;

namespace Unistream.TransactionsApi.V1.Queries
{
    /// <summary>
    /// Получение списка клиентов
    /// </summary>
    public class SearchClientsQuery : MediatR.IRequest<List<ClientModel>>
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
        public class SearchClientsQueryHandler : MediatR.IRequestHandler<SearchClientsQuery, List<ClientModel>>
        {
            /// <summary>
            /// Репозиторий клиентов
            /// </summary>
            private readonly IClientRepository _clientRepository;

            /// <summary>
            /// Конструктор
            /// </summary>
            public SearchClientsQueryHandler(
                IClientRepository clientRepository)
            {
                _clientRepository = clientRepository;
            }

            ///<inheritdoc/>
            public async Task<List<ClientModel>> Handle(SearchClientsQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var clients = await _clientRepository.SearchClientsAsync(request.Skip, request.Take, cancellationToken);

                    return clients
                        .Select(ContractsMapper.Map)
                        .ToList();
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(ex, string.Format("Ошибка запроса списка клиентов}"));
                    throw ErrorFactory.Create(ErrorCode.UnspecifiedError, ex.Message);
                }
            }
        }
    }
}