using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CashFlowly.Core.Application.DTOs.Gastos;
using CashFlowly.Core.Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CashFlowly.Core.Application.Services
{
    public class RecommendationService : IRecommendationService
    {
        private readonly IGastoService _gastoService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<RecommendationService> _logger;
        private readonly HttpClient _httpClient;

        public RecommendationService(IGastoService gastoService, IConfiguration configuration, ILogger<RecommendationService> logger, HttpClient httpClient)
        {
            _gastoService = gastoService;
            _configuration = configuration;
            _logger = logger;
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _configuration["OpenAI:ApiKey"]);
        }

        public async Task<string> GetRecommendationsAsync(int usuarioId)
        {
            try
            {
                var gastos = await _gastoService.ObtenerGastosPorUsuarioAsync(usuarioId);
                var prompt = GeneratePrompt(gastos);

                var requestData = new
                {
                    model = "gpt-3.5-turbo",
                    messages = new[] { new { role = "user", content = prompt } }
                };

                var json = JsonSerializer.Serialize(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync();
                var responseObject = JsonSerializer.Deserialize<OpenAIResponse>(responseString);

                return responseObject?.choices?.FirstOrDefault()?.message?.content;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener recomendaciones de OpenAI.");
                return "Ocurrió un error al obtener las recomendaciones.";
            }
        }

        private string GeneratePrompt(List<MostrarGastos> gastos)
        {
            var promptBuilder = new StringBuilder();
            promptBuilder.AppendLine("Analiza los siguientes gastos y proporciona recomendaciones para mejorar la gestión financiera:");
            foreach (var gasto in gastos)
            {
                promptBuilder.AppendLine($"- {gasto.Fecha.ToShortDateString()}: {gasto.Monto} en {gasto.Categoria ?? gasto.CategoriaPersonalizada} ({gasto.Cuenta})");
            }
            promptBuilder.AppendLine("\nProporciona recomendaciones concisas y prácticas.");
            return promptBuilder.ToString();
        }
    }

    public class OpenAIResponse
    {
        public List<Choice> choices { get; set; }

        public class Choice
        {
            public Message message { get; set; }
        }

        public class Message
        {
            public string content { get; set; }
        }
    }
}
