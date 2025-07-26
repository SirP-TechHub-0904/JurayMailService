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
    public sealed class UpdateAccountSubscriptionCommand : IRequest
    {
        public UpdateAccountSubscriptionCommand(AccountSubscription userSubscription)
        {
            AccountSubscription = userSubscription;
        }

        public AccountSubscription AccountSubscription { get; set; }


    }

    public class UpdateAccountSubscriptionCommandHandler : IRequestHandler<UpdateAccountSubscriptionCommand>
    {
        private readonly IAccountSubscriptionRepository _userSubscriptionRepository;

        public UpdateAccountSubscriptionCommandHandler(IAccountSubscriptionRepository userSubscriptionRepository)
        {
            _userSubscriptionRepository = userSubscriptionRepository;
        }

        public async Task Handle(UpdateAccountSubscriptionCommand request, CancellationToken cancellationToken)
        {

            await _userSubscriptionRepository.UpdateAsync(request.AccountSubscription);
        }
    }
}
