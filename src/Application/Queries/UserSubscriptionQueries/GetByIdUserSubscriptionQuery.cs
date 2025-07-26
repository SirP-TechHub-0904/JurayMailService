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
    public sealed class GetByIdAccountSubscriptionQuery : IRequest<AccountSubscription>
    {
        public GetByIdAccountSubscriptionQuery(long id)
        {
            Id = id;
        }

        public long Id { get; set; }

        public class GetByIdAccountSubscriptionQueryHandler : IRequestHandler<GetByIdAccountSubscriptionQuery, AccountSubscription>
        {
            private readonly IAccountSubscriptionRepository _userSubscriptionRepository;

            public GetByIdAccountSubscriptionQueryHandler(IAccountSubscriptionRepository userSubscriptionRepository)
            {
                _userSubscriptionRepository = userSubscriptionRepository;
            }

            public async Task<AccountSubscription> Handle(GetByIdAccountSubscriptionQuery request, CancellationToken cancellationToken)
            {
                return await _userSubscriptionRepository.GetByIdAsync(request.Id);

            }
        }
    }

}
