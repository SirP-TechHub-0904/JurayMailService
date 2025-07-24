using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.UserSubscriptionCommands
{
    public sealed class DeleteUserSubscriptionCommand : IRequest
    {
        public DeleteUserSubscriptionCommand(long id)
        {
            Id = id;
        }

        public long Id { get; set; }

    }

    public class DeleteUserSubscriptionCommandHandler : IRequestHandler<DeleteUserSubscriptionCommand>
    {
        private readonly IUserSubscriptionRepository _userSubscriptionRepository;

        public DeleteUserSubscriptionCommandHandler(IUserSubscriptionRepository userSubscriptionRepository)
        {
            _userSubscriptionRepository = userSubscriptionRepository;
        }

        public async Task Handle(DeleteUserSubscriptionCommand request, CancellationToken cancellationToken)
        {

            var userSubscription = await _userSubscriptionRepository.GetByIdAsync(request.Id);

            await _userSubscriptionRepository.RemoveAsync(userSubscription);

        }
    }
}
