using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;
using Unistream.Clients.Model.Interfaces;
using Unistream.TransactionsApi.ErrorHandling;
using Unistream.TransactionsApi.V1.Contracts;

namespace Unistream.TransactionsApi.V1.Queries
{
    /// <summary>
    /// Запрос баланса клиента
    /// </summary>
    public class ClientBalanceQuery : MediatR.IRequest<ClientBalanceModel>
    {
        /// <summary>
        /// Идентификатор клиента
        /// </summary>
        public Guid ClientId { get; set; }

        /// <summary>
        /// Запрос
        /// </summary>
        public class ClientBalanceQueryHandler : MediatR.IRequestHandler<ClientBalanceQuery, ClientBalanceModel>
        {
            /// <summary>
            /// Репозиторий клиентов
            /// </summary>
            private readonly IClientRepository _clientRepository;

            /// <summary>
            /// Конструктор
            /// </summary>
            public ClientBalanceQueryHandler(
                IClientRepository clientRepository)
            {
                _clientRepository = clientRepository;
            }

            ///<inheritdoc/>
            public async Task<ClientBalanceModel> Handle(ClientBalanceQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    await _clientRepository.GetClientBalanceByIdAsync(request.ClientId, cancellationToken);
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(ex, string.Format("Ошибка запроса баланса клиента Id {0}", request.ClientId));
                    throw ErrorFactory.Create(ErrorCode.UnspecifiedError, ex.Message);
                }

                return null;
            }
        }
    }
}