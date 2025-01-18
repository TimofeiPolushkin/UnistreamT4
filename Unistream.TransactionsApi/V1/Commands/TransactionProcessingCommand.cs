//using System;
//using System.Threading;
//using System.Threading.Tasks;
//using Unistream.Transactions.Model.Interfaces;

//namespace Unistream.TransactionsApi.V1.Commands
//{
//    public class TransactionProcessingCommand : MediatR.IRequest
//    {

//        public class TransactionProcessingCommandHandler : MediatR.IRequestHandler<TransactionProcessingCommand>
//        {
//            /// <summary>
//            /// Репозиторий транзакций
//            /// </summary>
//            private readonly ITransactionRepository _transactionRepository;

//            /// <summary>
//            /// Конструктор
//            /// </summary>
//            public TransactionProcessingCommandHandler(
//                ITransactionRepository transactionRepository)
//            {
//                _transactionRepository = transactionRepository;
//            }

//            ///<inheritdoc/>
//            public async Task<MediatR.Unit> Handle(TransactionProcessingCommand request, CancellationToken token)
//            {
//                try
//                {

//                }
//                catch (Exception ex)
//                {
//                    //Log.Error(ex, "Ошибка при изменении объекта МБ");
//                    throw;
//                }

//                return MediatR.Unit.Value;
//            }
//        }
//    }
//}