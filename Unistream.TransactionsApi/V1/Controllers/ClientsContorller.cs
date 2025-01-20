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
using Unistream.TransactionsApi.V1.Commands;
using System.Collections.Generic;

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
        /// Создание клиента
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPost("create")]
        [ProducesResponseType(typeof(ClientModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(TransactionsApiError), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateAsync([FromBody] CreateClientCommand request, CancellationToken cancellationToken)
        {
            return Ok(await _dispatcher.Send(request, cancellationToken));
        }

        /// <summary>
        /// Баланс клиента
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpGet("balance/{id}")]
        [ProducesResponseType(typeof(ClientBalanceModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(TransactionsApiError), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> BalanceAsync([Required] Guid id, CancellationToken cancellationToken)
        {
            return Ok(await _dispatcher.Send(new ClientBalanceQuery
            {
                ClientId = id,
            },
            cancellationToken));
        }

        /// <summary>
        /// Поиск клиентов
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPost("search")]
        [ProducesResponseType(typeof(List<ClientModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(TransactionsApiError), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SearchAsync([FromBody] SearchClientsQuery request, CancellationToken cancellationToken)
        {
            return Ok(await _dispatcher.Send(request, cancellationToken));
        }
    }
}
