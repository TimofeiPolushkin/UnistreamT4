using Microsoft.EntityFrameworkCore;

namespace Unistream.Transactions.Model.EF
{
    public interface ITransactionContextFactory
    {
        public TransactionContext CreateContext(DbContextOptions<TransactionContext> dbOptions);
    }
}
