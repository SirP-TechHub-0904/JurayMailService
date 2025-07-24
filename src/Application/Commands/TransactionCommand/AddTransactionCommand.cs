using Domain.Interfaces;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.TransactionCommands
{
    public sealed class AddTransactionCommand : IRequest
    {
        public AddTransactionCommand(Transaction transaction)
        {
            Transaction = transaction;
        }

        public Transaction Transaction { get; set; }


    }

    public class AddTransactionCommandHandler : IRequestHandler<AddTransactionCommand>
    {
        private readonly ITransactionRepository _transactionRepository;

        public AddTransactionCommandHandler(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task Handle(AddTransactionCommand request, CancellationToken cancellationToken)
        {

            await _transactionRepository.AddAsync(request.Transaction);


        }
    }
}
