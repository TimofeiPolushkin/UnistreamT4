using Microsoft.EntityFrameworkCore;
using Unistream.Core.Context;

namespace Unistream.Clients.Model.EF
{
    public class ClientContextFactory : ContextFactoryBase<ClientContext>, IClientContextFactory
    {
        /// <inheritdoc />
        public ClientContextFactory(DbContextOptions<ClientContext> options)
            : base(options)
        {
        }

        /// <inheritdoc />
        public override ClientContext CreateContext(DbContextOptions<ClientContext> dbOptions)
        {
            return new ClientContext(dbOptions);
        }
    }
}
