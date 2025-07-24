using Domain.Interfaces;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.WalletCommands
{
    public sealed class UpdateWalletCommand : IRequest
    {
        public UpdateWalletCommand(Wallet wallet)
        {
            Wallet = wallet;
        }

        public Wallet Wallet { get; set; }


    }

    public class UpdateWalletCommandHandler : IRequestHandler<UpdateWalletCommand>
    {
        private readonly IWalletRepository _walletRepository;

        public UpdateWalletCommandHandler(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public async Task Handle(UpdateWalletCommand request, CancellationToken cancellationToken)
        {

            await _walletRepository.UpdateAsync(request.Wallet);
        }
    }
}
