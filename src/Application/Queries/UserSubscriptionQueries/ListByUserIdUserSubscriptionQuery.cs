using Domain.Interfaces;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.AccountSubscriptionQueries
{
    public sealed class ListAllAccountSubscriptionQuery : IRequest<List<AccountSubscription>>
    {
        
         
        public class ListAllAccountSubscriptionQueryHandler : IRequestHandler<ListAllAccountSubscriptionQuery, List<AccountSubscription>>
        {
            private readonly IAccountSubscriptionRepository _userSubscriptionRepository;

            public ListAllAccountSubscriptionQueryHandler(IAccountSubscriptionRepository userSubscriptionRepository)
            {
                _userSubscriptionRepository = userSubscriptionRepository;
            }

            public async Task<List<AccountSubscription>> Handle(ListAllAccountSubscriptionQuery request, CancellationToken cancellationToken)
            {
                return await _userSubscriptionRepository.GetAllAsync();

            }
        }
    }

}
