using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Context;
using Infrastructure.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{

    public sealed class PlanRepository : Repository<Plan>, IPlanRepository
    {
        private readonly AppDBContext _context;

        public PlanRepository(AppDBContext context) : base(context)
        {
            _context = context;
        }

    }


    public sealed class PlanFeatureRepository : Repository<PlanFeature>, IPlanFeatureRepository
    {
        private readonly AppDBContext _context;

        public PlanFeatureRepository(AppDBContext context) : base(context)
        {
            _context = context;
        }

    }
}
