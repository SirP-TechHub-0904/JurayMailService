using Domain.Interfaces;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.UserSubscriptionCommands
{
    public sealed class UpdateUserSubscriptionCommand : IRequest
    {
        public UpdateUserSubscriptionCommand(UserSubscription userSubscription)
        {
            UserSubscription = userSubscription;
        }

        public UserSubscription UserSubscription { get; set; }


    }

    public class UpdateUserSubscriptionCommandHandler : IRequestHandler<UpdateUserSubscriptionCommand>
    {
        private readonly IUserSubscriptionRepository _userSubscriptionRepository;

        public UpdateUserSubscriptionCommandHandler(IUserSubscriptionRepository userSubscriptionRepository)
        {
            _userSubscriptionRepository = userSubscriptionRepository;
        }

        public async Task Handle(UpdateUserSubscriptionCommand request, CancellationToken cancellationToken)
        {

            await _userSubscriptionRepository.UpdateAsync(request.UserSubscription);
        }
    }
}
