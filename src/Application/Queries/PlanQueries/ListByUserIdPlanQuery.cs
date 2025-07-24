using Domain.Interfaces;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.PlanQueries
{
    public sealed class ListAllPlanQuery : IRequest<List<Plan>>
    {
        
         
        public class ListAllPlanQueryHandler : IRequestHandler<ListAllPlanQuery, List<Plan>>
        {
            private readonly IPlanRepository _planRepository;

            public ListAllPlanQueryHandler(IPlanRepository planRepository)
            {
                _planRepository = planRepository;
            }

            public async Task<List<Plan>> Handle(ListAllPlanQuery request, CancellationToken cancellationToken)
            {
                return await _planRepository.GetAllAsync();

            }
        }
    }

}
