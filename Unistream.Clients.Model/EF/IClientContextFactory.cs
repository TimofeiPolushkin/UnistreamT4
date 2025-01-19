using Microsoft.EntityFrameworkCore;

namespace Unistream.Clients.Model.EF
{
    public interface IClientContextFactory
    {
        public ClientContext CreateContext(DbContextOptions<ClientContext> dbOptions);
    }
}
