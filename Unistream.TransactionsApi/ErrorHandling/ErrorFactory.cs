using System;
using System.Collections.Generic;

namespace Unistream.TransactionsApi.ErrorHandling
{
    /// <summary>
    /// Фабрика исключений
    /// </summary>
    public static class ErrorFactory
    {
        private static Dictionary<ErrorCode, Func<string, TransactionsException>> _factoryMap = new Dictionary<ErrorCode, Func<string, TransactionsException>>
        {
            //TODO Локализация
            {ErrorCode.NotFound, (msg) => new TransactionsException(msg ?? "Запрашиваемый объект не найден", ErrorCode.NotFound) },
            {ErrorCode.UnspecifiedError, (msg) => new TransactionsException(msg ?? "Неизвестная ошибка", ErrorCode.UnspecifiedError) },
            {ErrorCode.WrongRequest, (msg) => new TransactionsException(msg ?? "Неверный запрос", ErrorCode.WrongRequest) },
            {ErrorCode.TransactionsApiError, (msg) => new TransactionsException(msg ?? "Ошибка сервиса управления транзакциями", ErrorCode.TransactionsApiError) }
        };

        /// <summary>
        /// Создать исключение
        /// </summary>
        /// <param name="errorCode">Код ошибки</param>
        /// <param name="message">Текст ошибки</param>
        /// <returns></returns>
        public static TransactionsException Create(ErrorCode errorCode, string message = null)
        {
            return _factoryMap[errorCode](message);
        }
    }
}
