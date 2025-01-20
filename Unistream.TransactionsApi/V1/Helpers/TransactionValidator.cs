using System;
using Unistream.Clients.Model.Models;
using Unistream.Transactions.Model.Interfaces;
using Unistream.Transactions.Model.Models.Enums;

namespace Unistream.TransactionsApi.V1.Helpers
{
    /// <summary>
    /// Валидация транзакций
    /// </summary>
    public static class TransactionValidator
    {
        /// <summary>
        /// Валидация транзакции
        /// </summary>
        public static ResultValidationModel TransactionValidate(ITransaction transaction,
            ClientModel client,
            TransactionOperationType transactionOperationType)
        {
            var resultValidation = new ResultValidationModel();

            if (transaction == null)
            {
                resultValidation.Errors.Add("Был передан пустой запрос");
            }

            if (transaction.Amount <= 0)
            {
                resultValidation.Errors.Add("Сумма в транзакции должна быть больше нуля");
            }

            if (transactionOperationType == TransactionOperationType.Debit && (client.Balance - transaction.Amount) < 0)
            {
                resultValidation.Errors.Add("Баланс клиента не может быть меньше нуля");
            }

            if (transaction.DateTime.ToUniversalTime() > DateTimeOffset.UtcNow)
            {
                resultValidation.Errors.Add("Дата и время транзакции превышает текущую дату и время");
            }

            return resultValidation;
        }
    }
}
