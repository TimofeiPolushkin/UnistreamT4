using Microsoft.EntityFrameworkCore;
using Unistream.Clients.Model.Interfaces;
using Unistream.Clients.Model.Models;
using Unistream.Transactions.Model.EF.Entities;
using Unistream.Transactions.Model.EF.Enums;
using Unistream.Transactions.Model.Helpers;
using Unistream.Transactions.Model.Interfaces;
using Unistream.Transactions.Model.Models;
using Unistream.Transactions.Model.Models.Enums;

namespace Unistream.Transactions.Model.EF
{
    /// <summary>
    /// Операции с транзакциями БД
    /// </summary>
    public class TransactionRepository : ITransactionRepository
    {
        protected readonly IDbContextFactory<TransactionContext> _transactionContextFactory;

        /// <summary>
        /// Репозиторий клиентов
        /// </summary>
        private readonly IClientRepository _clientRepository;

        public TransactionRepository(IDbContextFactory<TransactionContext> transactionContextFactory,
            IClientRepository clientRepository)
        {
            _transactionContextFactory = transactionContextFactory;
            _clientRepository = clientRepository;
        }

        public async Task<ClientActualBalanceModel> ProcessTransactionAsync(ITransaction transaction,
            TransactionOperationType transactionOperationType,
            CancellationToken cancellationToken)
        {
            if (transaction == null)
            {
                throw new Exception("Неверных запрос транзакции");
            }

            ClientModel client = await _clientRepository.GetClientByIdAsync(transaction.ClientId, cancellationToken);

            if (client == null)
            {
                throw new Exception($"Не найден клиент с Id {transaction.ClientId}");
            }

            DateTimeOffset utcNow = DateTimeOffset.UtcNow;

            TransactionHistoryType transactionType = Mapper.Map(transactionOperationType);

            using (TransactionContext transactionContext = _transactionContextFactory.CreateDbContext())
            {
                TransactionHistory transactionHistory = await transactionContext.TransactionsHistory
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t => t.UniId == transaction.Id, cancellationToken);

                if (transactionHistory != null)
                {
                    return new ClientActualBalanceModel
                    {
                        OperationDate = transactionHistory.TransactionDateTime,
                        Balance = client.Balance
                    };
                }

                decimal totalBalance = CalclulateClientBalance(client.Balance, transaction.Amount, transactionType);

                await _clientRepository.ChangeClientBalanceAsync(client.UniId, totalBalance, cancellationToken);

                await transactionContext.TransactionsHistory.AddAsync(new Entities.TransactionHistory
                {
                    Created = utcNow,
                    Updated = utcNow,
                    UniId = transaction.Id,
                    ClientId = transaction.ClientId,
                    Amount = transaction.Amount,
                    TransactionType = transactionType,
                    TransactionDateTime = transaction.DateTime,
                });

                //TODO Атомарно с изменением баланса клиента
                await transactionContext.SaveChangesAsync(cancellationToken);

                return new ClientActualBalanceModel
                {
                    OperationDate = transaction.DateTime,
                    Balance = totalBalance
                };
            }
        }

        public async Task<ClientActualBalanceModel> RevertTransactionAsync(Guid transactionId,
            CancellationToken cancellationToken)
        {
            DateTimeOffset utcNow = DateTimeOffset.UtcNow;

            using (TransactionContext transactionContext = _transactionContextFactory.CreateDbContext())
            {
                TransactionHistory existsTransaction = await transactionContext.TransactionsHistory.FirstOrDefaultAsync(t => t.UniId == transactionId);

                if (existsTransaction == null)
                {
                    throw new Exception($"Не найдена транзакция для отката Id {transactionId}");
                }

                decimal clientBalance = await _clientRepository.GetClientBalanceByIdAsync(existsTransaction.ClientId, cancellationToken);

                if (existsTransaction.IsRollback == true)
                {
                    return new ClientActualBalanceModel
                    {
                        Balance = clientBalance,
                        OperationDate = existsTransaction.RollbackDateTime ?? utcNow
                    };
                }

                decimal totalBalance = CalclulateClientBalance(clientBalance, existsTransaction.Amount, SwitchTransactionType(existsTransaction.TransactionType));

                clientBalance = await _clientRepository.ChangeClientBalanceAsync(existsTransaction.ClientId, totalBalance, cancellationToken);

                existsTransaction.Updated = utcNow;
                existsTransaction.RollbackDateTime = utcNow;
                existsTransaction.IsRollback = true;

                //TODO Атомарно с изменением баланса клиента
                await transactionContext.SaveChangesAsync(cancellationToken);

                return new ClientActualBalanceModel
                {
                    Balance = clientBalance,
                    OperationDate = existsTransaction.RollbackDateTime ?? utcNow
                };
            }
        }

        //TODO Добавить фильтры, сортировку
        ///<inheritdoc/>
        public async Task<List<TransactionHistoryModel>> SearchClientsAsync(int skip, int take, CancellationToken cancellationToken)
        {
            using (TransactionContext transactionContext = _transactionContextFactory.CreateDbContext())
            {
                return await transactionContext.TransactionsHistory
                    .AsNoTracking()
                    .Where(c => c.Deleted == null)
                    .OrderBy(c => c.Id)
                    .Skip(skip)
                    .Take(take)
                    .Select(Mapper.TransactionHistoryProjection)
                    .ToListAsync(cancellationToken);
            }
        }

        private decimal CalclulateClientBalance(decimal balance, decimal amount, TransactionHistoryType transactionType)
        {
            switch(transactionType)
            {
                case TransactionHistoryType.Credit:
                    return balance + amount;

                case TransactionHistoryType.Debit:
                    decimal newBalance = balance - amount;

                    if (newBalance < 0)
                    {
                        throw new Exception("Баланс клиента не может быть меньше нуля");
                    }

                    return newBalance;

                default:
                    throw new Exception("Неизвестный тип операции транзакции");
            }
        }

        private TransactionHistoryType SwitchTransactionType(TransactionHistoryType transactionType)
        {
            switch (transactionType)
            {
                case TransactionHistoryType.Credit:
                    return TransactionHistoryType.Debit;
                case TransactionHistoryType.Debit:
                    return TransactionHistoryType.Credit;
                default:
                    throw new Exception("Неподдерживаемая операция");
            }
        }
    }
}
