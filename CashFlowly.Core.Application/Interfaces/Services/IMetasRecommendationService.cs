﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowly.Core.Application.Interfaces.Services
{
    public interface IMetasRecommendationService
    {
        Task<string> GetGoalsRecommendationsAsync(int usuarioId);
    }
}
