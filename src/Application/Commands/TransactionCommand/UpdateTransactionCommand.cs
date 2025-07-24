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
    public sealed class UpdateTransactionCommand : IRequest
    {
        public UpdateTransactionCommand(Transaction transaction)
        {
            Transaction = transaction;
        }

        public Transaction Transaction { get; set; }


    }

    public class UpdateTransactionCommandHandler : IRequestHandler<UpdateTransactionCommand>
    {
        private readonly ITransactionRepository _transactionRepository;

        public UpdateTransactionCommandHandler(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
        {

            await _transactionRepository.UpdateAsync(request.Transaction);
        }
    }
}
