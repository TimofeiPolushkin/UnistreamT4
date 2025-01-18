using Microsoft.EntityFrameworkCore;
using Unistream.Transactions.Model.EF.Entities;
using Unistream.Transactions.Model.EF.EntityTypeConfiguration;

namespace Unistream.Transactions.Model.EF
{
    public class TransactionContext : DbContext
    {
        public DbSet<TransactionHistory> TransactionsHistory { get; set; }

        public TransactionContext(DbContextOptions<TransactionContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new TransactionHistoryConfiguration());
        }
    }
}
