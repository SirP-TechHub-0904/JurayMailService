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
    public sealed class GetByIdPlanQuery : IRequest<Plan>
    {
        public GetByIdPlanQuery(long id)
        {
            Id = id;
        }

        public long Id { get; set; }

        public class GetByIdPlanQueryHandler : IRequestHandler<GetByIdPlanQuery, Plan>
        {
            private readonly IPlanRepository _planRepository;

            public GetByIdPlanQueryHandler(IPlanRepository planRepository)
            {
                _planRepository = planRepository;
            }

            public async Task<Plan> Handle(GetByIdPlanQuery request, CancellationToken cancellationToken)
            {
                return await _planRepository.GetByIdAsync(request.Id);

            }
        }
    }

}
