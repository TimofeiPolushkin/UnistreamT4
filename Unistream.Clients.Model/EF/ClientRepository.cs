using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Unistream.Clients.Model.EF.Entities;
using Unistream.Clients.Model.Helpers;
using Unistream.Clients.Model.Interfaces;
using Unistream.Clients.Model.Models;

namespace Unistream.Clients.Model.EF
{
    /// <summary>
    /// Операции с клиентами БД
    /// </summary>
    public class ClientRepository : IClientRepository
    {
        protected readonly IDbContextFactory<ClientContext> _clientContextFactory;

        public ClientRepository(IDbContextFactory<ClientContext> clientContextFactory)
        {
            _clientContextFactory = clientContextFactory;
        }

        ///<inheritdoc/>
        public async Task<decimal> ChangeClientBalanceAsync(Guid id, decimal balance, CancellationToken cancellationToken)
        {
            using (ClientContext clientContext = _clientContextFactory.CreateDbContext())
            {
                DateTimeOffset utcNow = DateTimeOffset.UtcNow;

                Client client = await clientContext.Clients
                    .FirstOrDefaultAsync(c => c.UniId == id, cancellationToken);

                if (client == null)
                {
                    throw new Exception($"Не найден клиент с Id {id}");
                }

                client.Balance = balance;

                await clientContext.SaveChangesAsync(cancellationToken);

                return client.Balance;
            }
        }

        ///<inheritdoc/>
        public async Task<ClientModel> CreateOrUpdateClientAsync(ClientModel clientModel, CancellationToken cancellationToken)
        {
            using (ClientContext clientContext = _clientContextFactory.CreateDbContext())
            {
                DateTimeOffset utcNow = DateTimeOffset.UtcNow;

                if (clientModel?.UniId != null && clientModel.UniId != Guid.Empty)
                {
                    Client client = await clientContext.Clients
                        .FirstOrDefaultAsync(c => c.UniId == clientModel.UniId, cancellationToken);

                    if (client == null)
                    {
                        throw new Exception($"Не найден клиент с Id {clientModel.UniId}");
                    }

                    client.Updated = utcNow;
                    client.FirstName = clientModel.FirstName;
                    client.MiddleName = clientModel.MiddleName;
                    client.LastName = clientModel.LastName;

                    await clientContext.SaveChangesAsync(cancellationToken);

                    return Mapper.Map(client);
                }
                else
                {
                    var newClient = Mapper.Map(clientModel, utcNow);
                    newClient.UniId = Guid.NewGuid();

                    if (newClient == null)
                    {
                        throw new Exception($"Не указаны данные клиента при создании");
                    }

                    EntityEntry<Client> addedClient = await clientContext.Clients.AddAsync(newClient);

                    await clientContext.SaveChangesAsync(cancellationToken);

                    return Mapper.Map(addedClient.Entity);
                }
            }
        }

        ///<inheritdoc/>
        public async Task<ClientModel> GetClientByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            using (ClientContext clientContext = _clientContextFactory.CreateDbContext())
            {
                return await clientContext.Clients
                    .AsNoTracking()
                    .Where(c => c.Deleted == null)
                    .Select(Mapper.ClientProjection)
                    .FirstOrDefaultAsync(cancellationToken);
            }
        }

        ///<inheritdoc/>
        public async Task<decimal> GetClientBalanceByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            using (ClientContext clientContext = _clientContextFactory.CreateDbContext())
            {
                var clientBalance = await clientContext.Clients
                    .AsNoTracking()
                    .Where(c => c.Deleted == null)
                    .Select(c => new
                    {
                        c.UniId,
                        c.Balance
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                if (clientBalance == null)
                {
                    throw new Exception($"Не найден клиент с Id {id}");
                }

                return clientBalance.Balance;
            }
        }

        //TODO Добавить фильтры, сортировку
        ///<inheritdoc/>
        public async Task<List<ClientModel>> SearchClientsAsync(int skip, int take, CancellationToken cancellationToken)
        {
            using (ClientContext clientContext = _clientContextFactory.CreateDbContext())
            {
                return await clientContext.Clients
                    .AsNoTracking()
                    .Where(c => c.Deleted == null)
                    .OrderBy(c => c.Id)
                    .Skip(skip)
                    .Take(take)
                    .Select(Mapper.ClientProjection)
                    .ToListAsync(cancellationToken);
            }
        }
    }
}
