using Unistream.Clients.Model.Models;
using Unistream.TransactionsApi.V1.Contracts;

namespace Unistream.TransactionsApi.V1.Helpers
{
    /// <summary>
    /// Маппинг
    /// </summary>
    public static class ContractsMapper
    {
        public static ClientBalanceModel MapBalance(ClientActualBalanceModel model)
        {
            if (model == null)
            {
                return null;
            }

            return new ClientBalanceModel
            {
                BalanceDateTime = model.OperationDate,
                ClientBalance = model.Balance,
            };
        }

        public static InsertTransactionOperationResultModel MapInsertTransaction(ClientActualBalanceModel model)
        {
            if (model == null)
            {
                return null;
            }

            return new InsertTransactionOperationResultModel
            {
                InsertDateTime = model.OperationDate,
                ClientBalance = model.Balance,
            };
        }

        public static RevertTransactionOperationResultModel MapRevertTransaction(ClientActualBalanceModel model)
        {
            if (model == null)
            {
                return null;
            }

            return new RevertTransactionOperationResultModel
            {
                RevertDateTime = model.OperationDate,
                ClientBalance = model.Balance,
            };
        }

        public static TransactionsApi.V1.Contracts.ClientModel Map(Unistream.Clients.Model.Models.ClientModel model)
        {
            if (model == null)
            {
                return null;
            }

            return new TransactionsApi.V1.Contracts.ClientModel
            {
                UniId = model.UniId,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                Balance = model.Balance
            };
        }

        public static TransactionsApi.V1.Contracts.TransactionHistoryModel Map(Unistream.Transactions.Model.Models.TransactionHistoryModel model)
        {
            if (model == null)
            {
                return null;
            }

            return new TransactionsApi.V1.Contracts.TransactionHistoryModel
            {
                UniId = model.UniId,
                Amount = model.Amount,
                ClientId = model.ClientId,
                TransactionDateTime = model.TransactionDateTime,
                IsRollback = model.IsRollback,
                RollbackDateTime = model.RollbackDateTime
            };
        }
    }
}
