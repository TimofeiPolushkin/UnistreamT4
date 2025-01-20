using System.Linq.Expressions;
using Unistream.Clients.Model.EF.Entities;
using Unistream.Clients.Model.Models;

namespace Unistream.Clients.Model.Helpers
{
    public static class Mapper
    {
        public static ClientModel Map(Client entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new ClientModel
            {
                UniId = entity.UniId,
                FirstName = entity.FirstName,
                MiddleName = entity.MiddleName,
                LastName = entity.LastName,
                Balance = entity.Balance
            };
        }

        public static Client Map(ClientModel model, DateTimeOffset dateTime)
        {
            if (model == null)
            {
                return null;
            }

            return new Client
            {
                Created = dateTime,
                Updated = dateTime,
                UniId = model.UniId,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                Balance = model.Balance
            };
        }

        internal static Expression<Func<Client, ClientModel>> ClientProjection = (entity) => new ClientModel
        {
                UniId = entity.UniId,
                FirstName = entity.FirstName,
                MiddleName = entity.MiddleName,
                LastName = entity.LastName,
                Balance = entity.Balance
        };
    }
}
