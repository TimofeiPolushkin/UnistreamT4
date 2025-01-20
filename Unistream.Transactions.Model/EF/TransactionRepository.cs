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

                await transactionContext.SaveChangesAsync(cancellationToken);

                return new ClientActualBalanceModel
                {
                    OperationDate = transaction.DateTime,
                    Balance = client.Balance
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

                decimal totalBalance = CalclulateClientBalance(clientBalance, existsTransaction.Amount, existsTransaction.TransactionType, true);

                clientBalance = await _clientRepository.ChangeClientBalanceAsync(existsTransaction.ClientId, totalBalance, cancellationToken);

                existsTransaction.Updated = utcNow;
                existsTransaction.RollbackDateTime = utcNow;
                existsTransaction.IsRollback = true;

                await transactionContext.SaveChangesAsync(cancellationToken);

                return new ClientActualBalanceModel
                {
                    Balance = clientBalance,
                    OperationDate = existsTransaction.RollbackDateTime ?? utcNow
                };
            }
        }

        private decimal CalclulateClientBalance(decimal balance, decimal amount, TransactionHistoryType transactionType, bool IsSwapOperation = false)
        {
            switch(transactionType)
            {
                case TransactionHistoryType.Credit:
                    if (IsSwapOperation)
                    {
                        goto case TransactionHistoryType.Debit;
                    }

                    return balance + amount;

                case TransactionHistoryType.Debit:
                    if (IsSwapOperation)
                    {
                        goto case TransactionHistoryType.Credit;
                    }

                    decimal newBalance = balance + amount;

                    if (newBalance < 0)
                    {
                        throw new Exception("Баланс не может быть отрицательным");
                    }

                    return newBalance;

                default:
                    throw new Exception("Неизвестный тип операции транзакции");
            }
        }
    }
}
