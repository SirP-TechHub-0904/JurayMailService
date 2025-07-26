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
    public sealed class GetByIdPlanFeatureQuery : IRequest<PlanFeature>
    {
        public GetByIdPlanFeatureQuery(long id)
        {
            Id = id;
        }

        public long Id { get; set; }

        public class GetByIdPlanFeatureQueryHandler : IRequestHandler<GetByIdPlanFeatureQuery, PlanFeature>
        {
            private readonly IPlanFeatureRepository _planFeatureRepository;

            public GetByIdPlanFeatureQueryHandler(IPlanFeatureRepository planFeatureRepository)
            {
                _planFeatureRepository = planFeatureRepository;
            }

            public async Task<PlanFeature> Handle(GetByIdPlanFeatureQuery request, CancellationToken cancellationToken)
            {
                return await _planFeatureRepository.GetByIdAsync(request.Id);

            }
        }
    }

}
