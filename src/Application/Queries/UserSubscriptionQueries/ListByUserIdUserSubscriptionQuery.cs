using Domain.Interfaces;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.UserSubscriptionQueries
{
    public sealed class ListAllUserSubscriptionQuery : IRequest<List<UserSubscription>>
    {
        
         
        public class ListAllUserSubscriptionQueryHandler : IRequestHandler<ListAllUserSubscriptionQuery, List<UserSubscription>>
        {
            private readonly IUserSubscriptionRepository _userSubscriptionRepository;

            public ListAllUserSubscriptionQueryHandler(IUserSubscriptionRepository userSubscriptionRepository)
            {
                _userSubscriptionRepository = userSubscriptionRepository;
            }

            public async Task<List<UserSubscription>> Handle(ListAllUserSubscriptionQuery request, CancellationToken cancellationToken)
            {
                return await _userSubscriptionRepository.GetAllAsync();

            }
        }
    }

}
