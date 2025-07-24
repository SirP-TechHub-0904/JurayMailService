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
    public sealed class GetByIdUserSubscriptionQuery : IRequest<UserSubscription>
    {
        public GetByIdUserSubscriptionQuery(long id)
        {
            Id = id;
        }

        public long Id { get; set; }

        public class GetByIdUserSubscriptionQueryHandler : IRequestHandler<GetByIdUserSubscriptionQuery, UserSubscription>
        {
            private readonly IUserSubscriptionRepository _userSubscriptionRepository;

            public GetByIdUserSubscriptionQueryHandler(IUserSubscriptionRepository userSubscriptionRepository)
            {
                _userSubscriptionRepository = userSubscriptionRepository;
            }

            public async Task<UserSubscription> Handle(GetByIdUserSubscriptionQuery request, CancellationToken cancellationToken)
            {
                return await _userSubscriptionRepository.GetByIdAsync(request.Id);

            }
        }
    }

}
