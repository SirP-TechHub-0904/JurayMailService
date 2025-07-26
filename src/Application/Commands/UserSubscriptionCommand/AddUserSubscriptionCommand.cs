using Domain.Interfaces;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.AccountSubscriptionCommands
{
    public sealed class AddAccountSubscriptionCommand : IRequest
    {
        public AddAccountSubscriptionCommand(AccountSubscription accountSubscription)
        {
            AccountSubscription = accountSubscription;
        }

        public AccountSubscription AccountSubscription { get; set; }


    }

    public class AddAccountSubscriptionCommandHandler : IRequestHandler<AddAccountSubscriptionCommand>
    {
        private readonly IAccountSubscriptionRepository _userSubscriptionRepository;

        public AddAccountSubscriptionCommandHandler(IAccountSubscriptionRepository userSubscriptionRepository)
        {
            _userSubscriptionRepository = userSubscriptionRepository;
        }

        public async Task Handle(AddAccountSubscriptionCommand request, CancellationToken cancellationToken)
        {

            await _userSubscriptionRepository.AddAsync(request.AccountSubscription);


        }
    }
}
