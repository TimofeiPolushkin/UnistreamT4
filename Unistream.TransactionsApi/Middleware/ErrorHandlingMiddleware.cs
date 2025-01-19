using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Net;
using System.Threading.Tasks;
using Unistream.TransactionsApi.ErrorHandling;

namespace Unistream.TransactionsApi.Middleware
{
    /// <summary>
    /// Middleware обработки ошибок
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IOptions<MvcNewtonsoftJsonOptions> _jsonOptions;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="next">Делегат</param>
        /// <param name="jsonOptions">JSON опции</param>
        public ErrorHandlingMiddleware(RequestDelegate next, IOptions<MvcNewtonsoftJsonOptions> jsonOptions)
        {
            _next = next;
            _jsonOptions = jsonOptions;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode code;
            object errorObjeсt = new object();

            switch (exception)
            {
                case TransactionsException ex:
                    {
                        Log.Logger.Error("Ошибка: {errorMessage}", ex.Message);
                        errorObjeсt = ex.GetError();
                        code = HttpStatusCode.BadRequest;
                        break;
                    }
                default:
                    {
                        string absoluteURL = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";
                        Log.Logger.Error(exception, $"Произошла непредвиденная ошибка. Запрос: {absoluteURL}");
                        errorObjeсt = new TransactionsApiError { Message = exception.Message, ErrorCode = ErrorCode.UnspecifiedError };
                        code = HttpStatusCode.InternalServerError;
                        break;
                    }
            }

            var result = JsonConvert.SerializeObject(errorObjeсt, Formatting.Indented, _jsonOptions.Value.SerializerSettings);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
