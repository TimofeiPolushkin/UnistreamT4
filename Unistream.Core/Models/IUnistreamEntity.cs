namespace Unistream.Core.Models
{
    public interface IUnistreamEntity
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        long Id { get; set; }

        /// <summary>
        /// Сквозной идентификатор
        /// </summary>
        public Guid UniId { get; set; }

        /// <summary>
        /// Дата обновления
        /// </summary>
        DateTimeOffset? Updated { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        DateTimeOffset? Created { get; set; }

        /// <summary>
        /// Дата удаления
        /// </summary>
        DateTimeOffset? Deleted { get; set; }

        /// <summary>
        /// Версия
        /// </summary>
        byte[] Version { get; set; }
    }
}
