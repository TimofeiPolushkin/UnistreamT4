using System.Threading.Tasks;
using System.Threading;
using System;
using Unistream.Transactions.Model.Interfaces;

namespace Unistream.TransactionsApi.V1.Commands
{
    public class DebitTransactionCommand : MediatR.IRequest
    {
        public class DebitTransactionCommandHandler : MediatR.IRequestHandler<DebitTransactionCommand>
        {
            /// <summary>
            /// Репозиторий транзакций
            /// </summary>
            //private readonly ITransactionRepository _transactionRepository;

            /// <summary>
            /// Конструктор
            /// </summary>
            public DebitTransactionCommandHandler(
                /*ITransactionRepository transactionRepository*/)
            {
                //_transactionRepository = transactionRepository;
            }

            ///<inheritdoc/>
            public async Task<MediatR.Unit> Handle(DebitTransactionCommand request, CancellationToken token)
            {
                try
                {

                }
                catch (Exception ex)
                {
                    //Log.Error(ex, "Ошибка при изменении объекта МБ");
                    throw;
                }

                return MediatR.Unit.Value;
            }
        }
    }
}
