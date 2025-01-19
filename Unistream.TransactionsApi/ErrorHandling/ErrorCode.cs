namespace Unistream.TransactionsApi.ErrorHandling
{
    /// <summary>
    /// Коды ошибок
    /// </summary>
    public enum ErrorCode
    {
        /// <summary>
        /// Неизвестная ошибка
        /// </summary>
        UnspecifiedError = 1,

        /// <summary>
        /// Сущность не найдена
        /// </summary>
        NotFound,

        /// <summary>
        /// Неверный запрос
        /// </summary>
        WrongRequest,

        /// <summary>
        /// Ошибка сервиса управления транзакциями
        /// </summary>
        TransactionsApiError
    }
}
