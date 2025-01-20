using System.Threading.Tasks;
using System.Threading;
using System;
using Serilog;
using Unistream.Clients.Model.Models;
using Unistream.Clients.Model.Interfaces;
using System.Collections.Generic;
using Unistream.TransactionsApi.ErrorHandling;
using System.Linq;

namespace Unistream.TransactionsApi.V1.Commands
{
    /// <summary>
    /// Команда создания клиента
    /// </summary>
    public class CreateClientCommand : MediatR.IRequest<ClientModel>
    {
        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Баланс
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// Зачисление средств на счёт
        /// </summary>
        public class CreateClientCommandHandler : MediatR.IRequestHandler<CreateClientCommand, ClientModel>
        {
            private readonly IClientRepository _clientRepository;

            /// <summary>
            /// Конструктор
            /// </summary>
            public CreateClientCommandHandler(IClientRepository clientRepository)
            {
                _clientRepository = clientRepository;
            }

            ///<inheritdoc/>
            public async Task<ClientModel> Handle(CreateClientCommand request, CancellationToken cancellationToken)
            {
                List<string> errors = new List<string>();

                try
                {
                    if (string.IsNullOrEmpty(request.FirstName) || string.IsNullOrEmpty(request.MiddleName) || string.IsNullOrEmpty(request.LastName))
                    {
                        errors.Add("ФИО клиента должны быть заданы");
                    }

                    if (request.Balance < 0)
                    {
                        errors.Add("Баланс должен быть больше или равен нулю");
                    }

                    //TODO перенести в отельных класс валидации
                    if (errors != null && errors.Any())
                    {
                        string errorMessage = string.Join("\n", errors);
                        throw ErrorFactory.Create(ErrorCode.WrongRequest, errorMessage);
                    }

                    return await _clientRepository.CreateOrUpdateClientAsync(new ClientModel
                    {
                        FirstName = request.FirstName,
                        MiddleName = request.MiddleName,
                        LastName = request.LastName,
                        UniId = Guid.NewGuid(),
                        Balance = request.Balance
                    },
                    cancellationToken);
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
