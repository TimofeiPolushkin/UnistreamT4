using Unistream.Clients.Model.Models;

namespace Unistream.Clients.Model.Interfaces
{
    /// <summary>
    /// Операции с клиентами БД
    /// </summary>
    public interface IClientRepository
    {
        /// <summary>
        /// Получение списка клиентов
        /// </summary>
        public Task<List<ClientModel>> SearchClientsAsync(int skip,
            int take,
            CancellationToken cancellationToken);

        /// <summary>
        /// Получения клиента по Id
        /// </summary>
        public Task<ClientModel> GetClientByIdAsync(Guid id,
            CancellationToken cancellationToken);

        /// <summary>
        /// Получение баланса клиента
        /// </summary>
        public Task<decimal> GetClientBalanceByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Создание или изменение клиента
        /// </summary>
        public Task<ClientModel> CreateOrUpdateClientAsync(ClientModel clientModel,
            CancellationToken cancellationToken);

        /// <summary>
        /// Изменение баланса клиента
        /// </summary>
        public Task<decimal> ChangeClientBalanceAsync(Guid id,
            decimal balance,
            CancellationToken cancellationToken);
    }
}
