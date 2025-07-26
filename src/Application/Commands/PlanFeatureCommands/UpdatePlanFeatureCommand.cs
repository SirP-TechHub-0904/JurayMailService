using Domain.Interfaces;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.PlanFeatureCommands
{
    public sealed class UpdatePlanFeatureCommand : IRequest
    {
        public UpdatePlanFeatureCommand(PlanFeature planFeature)
        {
            PlanFeature = planFeature;
        }

        public PlanFeature PlanFeature { get; set; }


    }

    public class UpdatePlanFeatureCommandHandler : IRequestHandler<UpdatePlanFeatureCommand>
    {
        private readonly IPlanFeatureRepository _planFeatureRepository;

        public UpdatePlanFeatureCommandHandler(IPlanFeatureRepository planFeatureRepository)
        {
            _planFeatureRepository = planFeatureRepository;
        }

        public async Task Handle(UpdatePlanFeatureCommand request, CancellationToken cancellationToken)
        {

            await _planFeatureRepository.UpdateAsync(request.PlanFeature);
        }
    }
}
