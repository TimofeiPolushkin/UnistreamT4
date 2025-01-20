using Unistream.Transactions.Model.EF.Enums;
using Unistream.Transactions.Model.Models.Enums;

namespace Unistream.Transactions.Model.Helpers
{
    public static class Mapper
    {
        public static TransactionHistoryType Map(TransactionOperationType transactionOperationType)
        {
            switch(transactionOperationType)
            {
                case TransactionOperationType.Credit:
                    return TransactionHistoryType.Credit;
                case TransactionOperationType.Debit:
                    return TransactionHistoryType.Debit;
                default: throw new Exception("Неподдерживаемый тип транзакции");
            }
        }
    }
}
