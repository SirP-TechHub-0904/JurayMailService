using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.PlanCommands
{
    public sealed class DeletePlanCommand : IRequest
    {
        public DeletePlanCommand(long id)
        {
            Id = id;
        }

        public long Id { get; set; }

    }

    public class DeletePlanCommandHandler : IRequestHandler<DeletePlanCommand>
    {
        private readonly IPlanRepository _planRepository;

        public DeletePlanCommandHandler(IPlanRepository planRepository)
        {
            _planRepository = planRepository;
        }

        public async Task Handle(DeletePlanCommand request, CancellationToken cancellationToken)
        {

            var plan = await _planRepository.GetByIdAsync(request.Id);

            await _planRepository.RemoveAsync(plan);

        }
    }
}
