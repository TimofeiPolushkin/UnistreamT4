using System;

namespace Unistream.TransactionsApi.V1.Contracts
{
    /// <summary>
    /// Модель баланса клиента
    /// </summary>
    public class ClientBalanceModel
    {
        /// <summary>
        /// Дата получения баланса
        /// </summary>
        public DateTimeOffset BalanceDateTime { get; set; }

        /// <summary>
        /// Баланс клиента
        /// </summary>
        public decimal ClientBalance { get; set; }
    }
}
