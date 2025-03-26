using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashFlowly.Core.Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CashFlowly.Core.Application.Services
{
    public class MetasRecommendationService : IMetasRecommendationService
    {
        private readonly IMetaService _metaService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<MetasRecommendationService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public MetasRecommendationService(IMetaService metaService, IConfiguration configuration, ILogger<MetasRecommendationService> logger, IHttpClientFactory httpClientFactory)
        {
            _metaService = metaService;
            _configuration = configuration;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public Task<string> GetGoalsRecommendationsAsync(int usuarioId)
        {
            
        }
    }
}
