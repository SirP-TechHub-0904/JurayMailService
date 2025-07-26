using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.AccountSubscriptionCommands
{
    public sealed class DeleteAccountSubscriptionCommand : IRequest
    {
        public DeleteAccountSubscriptionCommand(long id)
        {
            Id = id;
        }

        public long Id { get; set; }

    }

    public class DeleteAccountSubscriptionCommandHandler : IRequestHandler<DeleteAccountSubscriptionCommand>
    {
        private readonly IAccountSubscriptionRepository _userSubscriptionRepository;

        public DeleteAccountSubscriptionCommandHandler(IAccountSubscriptionRepository userSubscriptionRepository)
        {
            _userSubscriptionRepository = userSubscriptionRepository;
        }

        public async Task Handle(DeleteAccountSubscriptionCommand request, CancellationToken cancellationToken)
        {

            var userSubscription = await _userSubscriptionRepository.GetByIdAsync(request.Id);

            await _userSubscriptionRepository.RemoveAsync(userSubscription);

        }
    }
}
