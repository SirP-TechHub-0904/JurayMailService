using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.PlanFeatureCommands
{
    public sealed class DeletePlanFeatureCommand : IRequest
    {
        public DeletePlanFeatureCommand(long id)
        {
            Id = id;
        }

        public long Id { get; set; }

    }

    public class DeletePlanFeatureCommandHandler : IRequestHandler<DeletePlanFeatureCommand>
    {
        private readonly IPlanFeatureRepository _planFeatureRepository;

        public DeletePlanFeatureCommandHandler(IPlanFeatureRepository planFeatureRepository)
        {
            _planFeatureRepository = planFeatureRepository;
        }

        public async Task Handle(DeletePlanFeatureCommand request, CancellationToken cancellationToken)
        {

            var planFeature = await _planFeatureRepository.GetByIdAsync(request.Id);

            await _planFeatureRepository.RemoveAsync(planFeature);

        }
    }
}
