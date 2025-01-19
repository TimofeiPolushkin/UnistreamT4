namespace Unistream.Transactions.Model.Interfaces
{
    /// <summary>
    /// Интерфейс транзакции
    /// </summary>
    public interface ITransaction
    {
        /// <summary>
        /// Идентификатор транзакции
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Идентификатор клиента
        /// </summary>
        public Guid ClientId { get; }

        /// <summary>
        /// Дата транзакции
        /// </summary>
        public DateTimeOffset DateTime { get; }

        /// <summary>
        /// Сумма в транзакции
        /// </summary>
        public decimal Amount { get; }
    }
}
