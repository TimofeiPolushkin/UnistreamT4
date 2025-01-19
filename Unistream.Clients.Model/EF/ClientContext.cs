using Microsoft.EntityFrameworkCore;
using Unistream.Clients.Model.EF.Entities;
using Unistream.Clients.Model.EF.EntityTypeConfiguration;

namespace Unistream.Clients.Model.EF
{
    /// <summary>
    /// Контекст клиентов
    /// </summary>
    public class ClientContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="options"></param>
        public ClientContext(DbContextOptions<ClientContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ClientConfiguration());
        }
    }
}
