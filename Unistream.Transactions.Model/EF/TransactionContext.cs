using Microsoft.EntityFrameworkCore;
using Unistream.Transactions.Model.EF.Entities;
using Unistream.Transactions.Model.EF.EntityTypeConfiguration;

namespace Unistream.Transactions.Model.EF
{
    /// <summary>
    /// Контекст транзакций
    /// </summary>
    public class TransactionContext : DbContext
    {
        public DbSet<TransactionHistory> TransactionsHistory { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="options"></param>
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
