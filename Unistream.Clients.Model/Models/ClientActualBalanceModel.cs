namespace Unistream.Clients.Model.Models
{
    /// <summary>
    /// Актуальный баланс клиента
    /// </summary>
    public class ClientActualBalanceModel
    {
        /// <summary>
        /// Время операции
        /// </summary>
        public DateTimeOffset OperationDate { get; set; }

        /// <summary>
        /// Баланс
        /// </summary>
        public decimal Balance { get; set; }
    }
}
