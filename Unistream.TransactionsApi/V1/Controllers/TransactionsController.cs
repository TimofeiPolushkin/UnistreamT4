using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Unistream.TransactionsApi.ErrorHandling;
using Unistream.TransactionsApi.V1.Commands;
using Unistream.TransactionsApi.V1.Contracts;
using Unistream.TransactionsApi.V1.Queries;

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
        /// <param name="cancellationToken">Токен отмены</param>
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
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPost("debit")]
        [ProducesResponseType(typeof(InsertTransactionOperationResultModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(TransactionsApiError), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DebitAsync([FromBody] DebitTransactionCommand request, CancellationToken cancellationToken)
        {
            return Ok(await _dispatcher.Send(request, cancellationToken));
        }

        /// <summary>
        /// Откат транзакции
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPost("revert/{id}")]
        [ProducesResponseType(typeof(RevertTransactionOperationResultModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(TransactionsApiError), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RevertCreditAsync([Required] Guid id, CancellationToken cancellationToken)
        {
            return Ok(await _dispatcher.Send(new RevertTransactionCommand
            {
                TransactionId = id,
            },
            cancellationToken));
        }

        /// <summary>
        /// Поиск истории транзакций
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPost("search")]
        [ProducesResponseType(typeof(List<TransactionHistoryModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(TransactionsApiError), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SearchAsync([FromBody] SearchTransactionsHistoryQuery request, CancellationToken cancellationToken)
        {
            return Ok(await _dispatcher.Send(request, cancellationToken));
        }
    }
}
