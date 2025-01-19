namespace Unistream.Clients.Model.EF.Entities
{
    /// <summary>
    /// Клиент
    /// </summary>
    public class Client
    {
        ///<inheritdoc/>
        public long Id { get; set; }

        ///<inheritdoc/>
        public Guid UniId { get; set; }

        ///<inheritdoc/>
        public DateTimeOffset? Created {  get; set; }

        ///<inheritdoc/>
        public DateTimeOffset? Updated { get; set; }

        ///<inheritdoc/>
        public DateTimeOffset? Deleted { get; set; }

        ///<inheritdoc/>
        public byte[] Version { get; set; } = Array.Empty<byte>();

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
