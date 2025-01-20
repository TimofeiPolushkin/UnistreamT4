using System.Collections.Generic;

namespace Unistream.TransactionsApi.V1.Helpers
{
    /// <summary>
    /// Результат валидации
    /// </summary>
    public class ResultValidationModel
    {
        /// <summary>
        /// Список ошибок
        /// </summary>
        public List<string> Errors { get; set; }

        /// <summary>
        /// Список предупреждений
        /// </summary>
        public List<string> Warnings { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public ResultValidationModel()
        {
            Errors = new List<string>();
            Warnings = new List<string>();
        }
    }
}
