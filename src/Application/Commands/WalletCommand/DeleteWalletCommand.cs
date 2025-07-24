using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.WalletCommands
{
    public sealed class DeleteWalletCommand : IRequest
    {
        public DeleteWalletCommand(long id)
        {
            Id = id;
        }

        public long Id { get; set; }

    }

    public class DeleteWalletCommandHandler : IRequestHandler<DeleteWalletCommand>
    {
        private readonly IWalletRepository _walletRepository;

        public DeleteWalletCommandHandler(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public async Task Handle(DeleteWalletCommand request, CancellationToken cancellationToken)
        {

            var wallet = await _walletRepository.GetByIdAsync(request.Id);

            await _walletRepository.RemoveAsync(wallet);

        }
    }
}
