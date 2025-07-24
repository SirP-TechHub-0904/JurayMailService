using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.TransactionCommands
{
    public sealed class DeleteTransactionCommand : IRequest
    {
        public DeleteTransactionCommand(long id)
        {
            Id = id;
        }

        public long Id { get; set; }

    }

    public class DeleteTransactionCommandHandler : IRequestHandler<DeleteTransactionCommand>
    {
        private readonly ITransactionRepository _transactionRepository;

        public DeleteTransactionCommandHandler(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
        {

            var transaction = await _transactionRepository.GetByIdAsync(request.Id);

            await _transactionRepository.RemoveAsync(transaction);

        }
    }
}
