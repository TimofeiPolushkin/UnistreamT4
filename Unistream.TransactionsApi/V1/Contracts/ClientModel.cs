using System;

namespace Unistream.TransactionsApi.V1.Contracts
{
    /// <summary>
    /// Клиент
    /// </summary>
    public class ClientModel
    {
        ///<inheritdoc/>
        public Guid UniId { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Баланс
        /// </summary>
        public decimal Balance { get; set; }
    }
}
