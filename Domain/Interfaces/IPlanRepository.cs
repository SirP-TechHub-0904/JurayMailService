﻿using Domain.GenericInterface;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IPlanRepository : IRepository<Plan>         
    {
    }

    public interface IPlanFeatureRepository : IRepository<PlanFeature>
    {
    }
}
