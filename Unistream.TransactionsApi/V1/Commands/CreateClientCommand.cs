using System.Threading.Tasks;
using System.Threading;
using System;
using Unistream.Transactions.Model.Interfaces;
using Serilog;
using Unistream.TransactionsApi.Services;
using Unistream.Transactions.Model.Models.Enums;
using Unistream.Clients.Model.Models;

namespace Unistream.TransactionsApi.V1.Commands
{
    /// <summary>
    /// Команда создания клиента
    /// </summary>
    public class CreateClientCommand : MediatR.IRequest<ClientModel>
    {
        ///<inheritdoc/>
        public Guid ClientId { get; set; }

        ///<inheritdoc/>
        public DateTimeOffset DateTime { get; set; }

        /// <summary>
        /// Зачисление средств на счёт
        /// </summary>
        public class CreateClientCommandHandler : MediatR.IRequestHandler<CreateClientCommand, ClientModel>
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public CreateClientCommandHandler()
            {
            }

            ///<inheritdoc/>
            public async Task<ClientModel> Handle(CreateClientCommand request, CancellationToken token)
            {
                try
                {
                    return new ClientModel();
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(ex, "Ошибка создания клиента");
                    throw;
                }
            }
        }
    }
}
