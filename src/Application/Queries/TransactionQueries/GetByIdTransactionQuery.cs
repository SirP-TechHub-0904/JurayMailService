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
    public sealed class GetByIdTransactionQuery : IRequest<Transaction>
    {
        public GetByIdTransactionQuery(long id)
        {
            Id = id;
        }

        public long Id { get; set; }

        public class GetByIdTransactionQueryHandler : IRequestHandler<GetByIdTransactionQuery, Transaction>
        {
            private readonly ITransactionRepository _transactionRepository;

            public GetByIdTransactionQueryHandler(ITransactionRepository transactionRepository)
            {
                _transactionRepository = transactionRepository;
            }

            public async Task<Transaction> Handle(GetByIdTransactionQuery request, CancellationToken cancellationToken)
            {
                return await _transactionRepository.GetByIdAsync(request.Id);

            }
        }
    }

}
