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
    public sealed class AddPlanFeatureCommand : IRequest
    {
        public AddPlanFeatureCommand(PlanFeature planFeature)
        {
            PlanFeature = planFeature;
        }

        public PlanFeature PlanFeature { get; set; }


    }

    public class AddPlanFeatureCommandHandler : IRequestHandler<AddPlanFeatureCommand>
    {
        private readonly IPlanFeatureRepository _planFeatureRepository;

        public AddPlanFeatureCommandHandler(IPlanFeatureRepository planFeatureRepository)
        {
            _planFeatureRepository = planFeatureRepository;
        }

        public async Task Handle(AddPlanFeatureCommand request, CancellationToken cancellationToken)
        {

            await _planFeatureRepository.AddAsync(request.PlanFeature);


        }
    }
}
