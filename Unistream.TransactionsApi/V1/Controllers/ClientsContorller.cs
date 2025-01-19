using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using Unistream.TransactionsApi.V1.Commands;

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
        /// Создание клиентов
        /// </summary>
        /// <param name="request">Запрос поиска</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync([FromBody] CreditTransactionCommand request, CancellationToken cancellationToken)
        {
            return Ok(await _dispatcher.Send(request, cancellationToken));
        }
    }
}
