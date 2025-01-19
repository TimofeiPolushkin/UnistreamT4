using Unistream.Transactions.Model.Models;
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
    }
}
