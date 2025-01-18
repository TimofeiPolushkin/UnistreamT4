using System.Threading.Tasks;
using System.Threading;
using System;
using Unistream.Transactions.Model.Interfaces;

namespace Unistream.TransactionsApi.V1.Commands
{
    public class CreditTransactionCommand : MediatR.IRequest
    {
        public class CreditTransactionCommandHandler : MediatR.IRequestHandler<CreditTransactionCommand>
        {
            /// <summary>
            /// Репозиторий транзакций
            /// </summary>
            //private readonly ITransactionRepository _transactionRepository;

            /// <summary>
            /// Конструктор
            /// </summary>
            public CreditTransactionCommandHandler(
                /*ITransactionRepository transactionRepository*/)
            {
                //_transactionRepository = transactionRepository;
            }

            ///<inheritdoc/>
            public async Task<MediatR.Unit> Handle(CreditTransactionCommand request, CancellationToken token)
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
