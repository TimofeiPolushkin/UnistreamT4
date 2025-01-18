using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Unistream.Transactions.Model.Interfaces;

namespace Unistream.Transactions.Model.EF
{
    public class TransactionRepository : ITransactionRepository
    {
        private const int OptimisticLockAttempts = 5;
        private const int OptimisticAttemptDelay = 300;

        protected readonly IDbContextFactory<TransactionContext> _contextFactory;
        private IServiceScopeFactory _serviceScopeFactory;

        public TransactionRepository(IDbContextFactory<TransactionContext> contextFactory,
            IServiceScopeFactory serviceScopeFactory)
        {
            _contextFactory = contextFactory;
            _serviceScopeFactory = serviceScopeFactory;
        }
    }
}
