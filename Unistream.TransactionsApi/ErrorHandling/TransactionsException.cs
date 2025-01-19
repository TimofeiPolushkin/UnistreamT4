using System;

namespace Unistream.TransactionsApi.ErrorHandling
{
    /// <summary>
    /// Базовое исключение API
    /// </summary>
    public class TransactionsException : Exception
    {
        /// <summary>
        /// Код ошибки
        /// </summary>
        public ErrorCode ErrorCode { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="message">Сообщение об ошибке</param>
        /// <param name="code">Код ошибки</param>
        public TransactionsException(string message, ErrorCode code) : base(message)
        {
            ErrorCode = code;
        }

        /// <summary>
        /// Получить ошибку
        /// </summary>
        /// <returns></returns>
        public TransactionsApiError GetError()
        {
            return new TransactionsApiError { ErrorCode = ErrorCode, Message = Message };
        }
    }
}
