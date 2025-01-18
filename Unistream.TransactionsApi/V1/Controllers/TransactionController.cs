using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Unistream.TransactionsApi.V1.Commands;

namespace Unistream.TransactionsApi.V1.Controllers
{
    /// <summary>
    /// Управление заказами
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TransactionController : ControllerBase
    {
        /// <summary>
        /// Диспатчер
        /// </summary>
        private readonly IMediator _dispatcher;

        /// <summary>
        /// Конструктор
        /// </summary>
        public TransactionController(IMediator dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpPost("credit")]
        public async Task<IActionResult> CreditAsync(CreditTransactionCommand request, CancellationToken token)
        {
            return Ok(await _dispatcher.Send(request, token));
        }

        [HttpPost("debit")]
        public async Task<IActionResult> DebitAsync(DebitTransactionCommand request, CancellationToken token)
        {
            return Ok(await _dispatcher.Send(request, token));
        }

        [HttpPost("revert/{id}")]
        public async Task<IActionResult> RevertCreditAsync(Guid id)
        {
            return Ok();
        }

        [HttpGet("balance/{id}")]
        public async Task<IActionResult> BalanceAsync(Guid id)
        {
            return Ok(10);
        }
    }
}
