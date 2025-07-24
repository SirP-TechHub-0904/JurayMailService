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
    public sealed class AddPlanCommand : IRequest
    {
        public AddPlanCommand(Plan plan)
        {
            Plan = plan;
        }

        public Plan Plan { get; set; }


    }

    public class AddPlanCommandHandler : IRequestHandler<AddPlanCommand>
    {
        private readonly IPlanRepository _planRepository;

        public AddPlanCommandHandler(IPlanRepository planRepository)
        {
            _planRepository = planRepository;
        }

        public async Task Handle(AddPlanCommand request, CancellationToken cancellationToken)
        {

            await _planRepository.AddAsync(request.Plan);


        }
    }
}
