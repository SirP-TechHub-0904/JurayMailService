using Domain.Interfaces;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.WalletQueries
{
    public sealed class GetByIdWalletQuery : IRequest<Wallet>
    {
        public GetByIdWalletQuery(long id)
        {
            Id = id;
        }

        public long Id { get; set; }

        public class GetByIdWalletQueryHandler : IRequestHandler<GetByIdWalletQuery, Wallet>
        {
            private readonly IWalletRepository _walletRepository;

            public GetByIdWalletQueryHandler(IWalletRepository walletRepository)
            {
                _walletRepository = walletRepository;
            }

            public async Task<Wallet> Handle(GetByIdWalletQuery request, CancellationToken cancellationToken)
            {
                return await _walletRepository.GetByIdAsync(request.Id);

            }
        }
    }

}
