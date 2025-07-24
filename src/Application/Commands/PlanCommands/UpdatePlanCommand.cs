using Domain.Interfaces;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.PlanCommands
{
    public sealed class UpdatePlanCommand : IRequest
    {
        public UpdatePlanCommand(Plan plan)
        {
            Plan = plan;
        }

        public Plan Plan { get; set; }


    }

    public class UpdatePlanCommandHandler : IRequestHandler<UpdatePlanCommand>
    {
        private readonly IPlanRepository _planRepository;

        public UpdatePlanCommandHandler(IPlanRepository planRepository)
        {
            _planRepository = planRepository;
        }

        public async Task Handle(UpdatePlanCommand request, CancellationToken cancellationToken)
        {

            await _planRepository.UpdateAsync(request.Plan);
        }
    }
}
