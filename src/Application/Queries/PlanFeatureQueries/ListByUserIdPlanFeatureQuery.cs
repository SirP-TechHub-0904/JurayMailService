using Domain.Interfaces;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.PlanFeatureQueries
{
    public sealed class ListAllPlanFeatureQuery : IRequest<List<PlanFeature>>
    {
        
         
        public class ListAllPlanFeatureQueryHandler : IRequestHandler<ListAllPlanFeatureQuery, List<PlanFeature>>
        {
            private readonly IPlanFeatureRepository _planFeatureRepository;

            public ListAllPlanFeatureQueryHandler(IPlanFeatureRepository planFeatureRepository)
            {
                _planFeatureRepository = planFeatureRepository;
            }

            public async Task<List<PlanFeature>> Handle(ListAllPlanFeatureQuery request, CancellationToken cancellationToken)
            {
                return await _planFeatureRepository.GetAllAsync();

            }
        }
    }

}
