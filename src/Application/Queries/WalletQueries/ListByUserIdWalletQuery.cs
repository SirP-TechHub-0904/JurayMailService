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
    public sealed class ListAllWalletQuery : IRequest<List<Wallet>>
    {
        
         
        public class ListAllWalletQueryHandler : IRequestHandler<ListAllWalletQuery, List<Wallet>>
        {
            private readonly IWalletRepository _walletRepository;

            public ListAllWalletQueryHandler(IWalletRepository walletRepository)
            {
                _walletRepository = walletRepository;
            }

            public async Task<List<Wallet>> Handle(ListAllWalletQuery request, CancellationToken cancellationToken)
            {
                return await _walletRepository.GetAllAsync();

            }
        }
    }

}
