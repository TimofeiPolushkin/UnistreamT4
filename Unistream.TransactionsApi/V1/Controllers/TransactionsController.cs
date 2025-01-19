using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Unistream.TransactionsApi.ErrorHandling;
using Unistream.TransactionsApi.V1.Commands;
using Unistream.TransactionsApi.V1.Contracts;

namespace Unistream.TransactionsApi.V1.Controllers
{
    /// <summary>
    /// Управление транзакциями
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TransactionsController : ControllerBase
    {
        /// <summary>
        /// Диспатчер
        /// </summary>
        private readonly IMediator _dispatcher;

        /// <summary>
        /// Конструктор
        /// </summary>
        public TransactionsController(IMediator dispatcher)
        {
            _dispatcher = dispatcher;
        }

        /// <summary>
        /// Зачисление средств клиенту
        /// </summary>
        /// <param name="request">Запрос поиска</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        [HttpPost("credit")]
        [ProducesResponseType(typeof(InsertTransactionOperationResultModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(TransactionsApiError), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreditAsync([FromBody] CreditTransactionCommand request, CancellationToken cancellationToken)
        {
            return Ok(await _dispatcher.Send(request, cancellationToken));
        }

        /// <summary>
        /// Списание средств клиента
        /// </summary>
        /// <param name="request">Запрос поиска</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        [HttpPost("debit")]
        [ProducesResponseType(typeof(InsertTransactionOperationResultModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(TransactionsApiError), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DebitAsync([FromBody] DebitTransactionCommand request, CancellationToken cancellationToken)
        {
            return Ok(await _dispatcher.Send(request, cancellationToken));
        }

        [HttpPost("revert/{id}")]
        [ProducesResponseType(typeof(RevertTransactionOperationResultModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(TransactionsApiError), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RevertCreditAsync([Required][FromQuery] Guid id, CancellationToken cancellationToken)
        {
            return Ok(await _dispatcher.Send(new RevertTransactionCommand
            {
                TransactionId = id,
            },
            cancellationToken));
        }

        [HttpGet("balance/{id}")]
        [ProducesResponseType(typeof(ClientBalanceModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(TransactionsApiError), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> BalanceAsync([Required][FromQuery] Guid id, CancellationToken cancellationToken)
        {
            return Ok(10);
        }
    }
}
