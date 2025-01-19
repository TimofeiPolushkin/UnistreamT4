using Microsoft.EntityFrameworkCore;
using Unistream.Clients.Model.EF;
using Unistream.Clients.Model.EF.Entities;
using Unistream.Transactions.Model.EF.Entities;
using Unistream.Transactions.Model.Interfaces;
using Unistream.Transactions.Model.Models;

namespace Unistream.Transactions.Model.EF
{
    /// <summary>
    /// Операции с транзакциями БД
    /// </summary>
    public class TransactionRepository : ITransactionRepository
    {
        protected readonly IDbContextFactory<TransactionContext> _transactionContextFactory;
        protected readonly IDbContextFactory<ClientContext> _clientContextFactory;

        public TransactionRepository(IDbContextFactory<TransactionContext> transactionContextFactory,
            IDbContextFactory<ClientContext> clientContextFactory)
        {
            _transactionContextFactory = transactionContextFactory;
            _clientContextFactory = clientContextFactory;
        }

        public async Task<ClientActualBalanceModel> CreditTransactionAsync(ITransaction transaction,
            CancellationToken cancellationToken)
        {
            DateTimeOffset utcNow = DateTimeOffset.UtcNow;

            using (ClientContext clientContext = _clientContextFactory.CreateDbContext())
            using (TransactionContext transactionContext = _transactionContextFactory.CreateDbContext())
            {
                TransactionHistory transactionHistory = await transactionContext.TransactionsHistory
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t => t.UniId == transaction.Id, cancellationToken);

                Client client = await clientContext.Clients
                    .FirstOrDefaultAsync(t => t.UniId == transaction.ClientId, cancellationToken);

                if (client == null)
                {
                    throw new Exception($"Не найден клиент с Id {transaction.ClientId}");
                }

                if (transactionHistory != null)
                {
                    return new ClientActualBalanceModel
                    {
                        OperationDate = transactionHistory.TransactionDateTime,
                        Balance = client.Balance
                    };
                }

                decimal totalBalance = client.Balance += transaction.Amount;

                await transactionContext.TransactionsHistory.AddAsync(new Entities.TransactionHistory
                {
                    Created = utcNow,
                    Updated = utcNow,
                    UniId = transaction.Id,
                    ClientId = transaction.ClientId,
                    Amount = transaction.Amount,
                    TransactionType = Enums.TransactionHistoryType.Credit,
                    TransactionDateTime = transaction.DateTime,
                });

                await clientContext.SaveChangesAsync(cancellationToken);
                await transactionContext.SaveChangesAsync(cancellationToken);

                return new ClientActualBalanceModel
                {
                    OperationDate = transaction.DateTime,
                    Balance = client.Balance
                };
            }
        }

        public async Task<ClientActualBalanceModel> DebitTransactionAsync(ITransaction transaction,
            CancellationToken cancellationToken)
        {
            DateTimeOffset utcNow = DateTimeOffset.UtcNow;

            using (ClientContext clientContext = _clientContextFactory.CreateDbContext())
            using (TransactionContext transactionContext = _transactionContextFactory.CreateDbContext())
            {
                TransactionHistory transactionHistory = await transactionContext.TransactionsHistory
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t => t.UniId == transaction.Id, cancellationToken);

                Client client = await clientContext.Clients
                    .FirstOrDefaultAsync(t => t.UniId == transaction.ClientId, cancellationToken);

                if (client == null)
                {
                    throw new Exception($"Не найден клиент с Id {transaction.ClientId}");
                }

                if (transactionHistory != null)
                {
                    return new ClientActualBalanceModel
                    {
                        OperationDate = transactionHistory.TransactionDateTime,
                        Balance = client.Balance
                    };
                }

                decimal totalBalance = client.Balance -= transaction.Amount;

                if (totalBalance < 0)
                {
                    throw new Exception("Баланс не может быть отрицательным");
                }

                await transactionContext.TransactionsHistory.AddAsync(new Entities.TransactionHistory
                {
                    Created = utcNow,
                    Updated = utcNow,
                    UniId = transaction.Id,
                    ClientId = transaction.ClientId,
                    Amount = transaction.Amount,
                    TransactionType = Enums.TransactionHistoryType.Debit,
                    TransactionDateTime = transaction.DateTime,
                });

                await clientContext.SaveChangesAsync(cancellationToken);
                await transactionContext.SaveChangesAsync(cancellationToken);

                return new ClientActualBalanceModel
                {
                    OperationDate = transaction.DateTime,
                    Balance = client.Balance
                };
            }
        }
    }
}
