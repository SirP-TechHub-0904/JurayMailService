using Domain.Interfaces;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.TransactionQueries
{
    public sealed class ListAllTransactionQuery : IRequest<List<Transaction>>
    {
        
         
        public class ListAllTransactionQueryHandler : IRequestHandler<ListAllTransactionQuery, List<Transaction>>
        {
            private readonly ITransactionRepository _transactionRepository;

            public ListAllTransactionQueryHandler(ITransactionRepository transactionRepository)
            {
                _transactionRepository = transactionRepository;
            }

            public async Task<List<Transaction>> Handle(ListAllTransactionQuery request, CancellationToken cancellationToken)
            {
                return await _transactionRepository.GetAllAsync();

            }
        }
    }

}
