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
    public sealed class AddUserSubscriptionCommand : IRequest
    {
        public AddUserSubscriptionCommand(UserSubscription userSubscription)
        {
            UserSubscription = userSubscription;
        }

        public UserSubscription UserSubscription { get; set; }


    }

    public class AddUserSubscriptionCommandHandler : IRequestHandler<AddUserSubscriptionCommand>
    {
        private readonly IUserSubscriptionRepository _userSubscriptionRepository;

        public AddUserSubscriptionCommandHandler(IUserSubscriptionRepository userSubscriptionRepository)
        {
            _userSubscriptionRepository = userSubscriptionRepository;
        }

        public async Task Handle(AddUserSubscriptionCommand request, CancellationToken cancellationToken)
        {

            await _userSubscriptionRepository.AddAsync(request.UserSubscription);


        }
    }
}
