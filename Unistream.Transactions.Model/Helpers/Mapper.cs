using System.Linq.Expressions;
using Unistream.Transactions.Model.EF.Entities;
using Unistream.Transactions.Model.EF.Enums;
using Unistream.Transactions.Model.Models;
using Unistream.Transactions.Model.Models.Enums;

namespace Unistream.Transactions.Model.Helpers
{
    public static class Mapper
    {
        public static TransactionHistoryType Map(TransactionOperationType transactionOperationType)
        {
            switch (transactionOperationType)
            {
                case TransactionOperationType.Credit:
                    return TransactionHistoryType.Credit;
                case TransactionOperationType.Debit:
                    return TransactionHistoryType.Debit;
                default: throw new Exception("Неподдерживаемый тип транзакции");
            }
        }

        internal static Expression<Func<TransactionHistory, TransactionHistoryModel>> TransactionHistoryProjection = (entity) => new TransactionHistoryModel
        {
            UniId = entity.UniId,
            Created = entity.Created,
            Updated = entity.Updated,
            Deleted = entity.Deleted,
            TransactionType = entity.TransactionType,
            Amount = entity.Amount,
            ClientId = entity.ClientId,
            TransactionDateTime = entity.TransactionDateTime,
            IsRollback = entity.IsRollback,
            RollbackDateTime = entity.RollbackDateTime
        };
    }
}
