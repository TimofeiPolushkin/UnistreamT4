using Microsoft.EntityFrameworkCore;
using Unistream.Core.Context;

namespace Unistream.Transactions.Model.EF
{
    public class TransactionContextFactory : ContextFactoryBase<TransactionContext>, ITransactionContextFactory
    {
        /// <inheritdoc />
        public TransactionContextFactory(DbContextOptions<TransactionContext> options)
            : base(options)
        {
        }

        /// <inheritdoc />
        public override TransactionContext CreateContext(DbContextOptions<TransactionContext> dbOptions)
        {
            return new TransactionContext(dbOptions);
        }
    }
}
