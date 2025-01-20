using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Unistream.TransactionsApi.ErrorHandling;
using Unistream.TransactionsApi.V1.Contracts;
using Unistream.TransactionsApi.V1.Queries;

namespace Unistream.TransactionsApi.V1.Controllers
{
    //TODO перенести в отдельное Api клиентов

    /// <summary>
    /// Управление клиентами
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ClientsController : ControllerBase
    {
        /// <summary>
        /// Диспатчер
        /// </summary>
        private readonly IMediator _dispatcher;

        /// <summary>
        /// Конструктор
        /// </summary>
        public ClientsController(IMediator dispatcher)
        {
            _dispatcher = dispatcher;
        }

        /// <summary>
        /// Баланс клиента
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpGet("balance/{id}")]
        [ProducesResponseType(typeof(ClientBalanceModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(TransactionsApiError), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> BalanceAsync([Required][FromQuery] Guid id, CancellationToken cancellationToken)
        {
            return Ok(await _dispatcher.Send(new ClientBalanceQuery
            {
                ClientId = id,
            },
            cancellationToken));
        }
    }
}
