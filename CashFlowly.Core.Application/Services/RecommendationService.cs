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
using System.Net.Http.Headers;

namespace CashFlowly.Core.Application.Services
{
    public class RecommendationService : IRecommendationService
    {
        private readonly IGastoService _gastoService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<RecommendationService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public RecommendationService(IGastoService gastoService, IConfiguration configuration, ILogger<RecommendationService> logger, IHttpClientFactory httpClientFactory)
        {
            _gastoService = gastoService;
            _configuration = configuration;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetRecommendationsAsync(int usuarioId)
        {
            _logger.LogInformation($"Iniciando obtención de recomendaciones para usuario {usuarioId}.");
            try
            {
                _logger.LogInformation($"Obteniendo gastos para usuario {usuarioId}.");
                var gastos = await _gastoService.ObtenerGastosPorUsuarioAsync(usuarioId);
                _logger.LogInformation($"Gastos obtenidos para usuario {usuarioId}. Cantidad: {gastos?.Count ?? 0}");

                if (gastos == null || !gastos.Any())
                {
                    _logger.LogWarning($"No se encontraron gastos para el usuario {usuarioId}.");
                    return "No se encontraron gastos para generar recomendaciones.";
                }

                var prompt = GeneratePrompt(gastos);
                _logger.LogDebug($"Prompt generado: {prompt}");

                var requestData = new
                {
                    model = "gpt-4o",
                    messages = new[] { new { role = "user", content = prompt } }
                };

                var json = JsonSerializer.Serialize(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var httpClient = _httpClientFactory.CreateClient("OpenAIClient");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _configuration["OpenAI:ApiKey"]);

                _logger.LogInformation("Enviando petición a la API de OpenAI.");
                var response = await httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);

                _logger.LogInformation($"Respuesta de OpenAI: {response.StatusCode}");
                var responseString = await response.Content.ReadAsStringAsync();
                _logger.LogDebug($"Cuerpo de respuesta de OpenAI: {responseString}");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Error en la API de OpenAI: {response.StatusCode} - {responseString}");
                    return "Error al obtener recomendaciones.";
                }

                try
                {
                    var responseObject = JsonSerializer.Deserialize<OpenAIResponse>(responseString);
                    var recommendation = responseObject?.choices?.FirstOrDefault()?.message?.content;
                    _logger.LogInformation("Recomendaciones obtenidas de OpenAI.");
                    return recommendation;
                }
                catch (JsonException ex)
                {
                    _logger.LogError(ex, $"Error al deserializar la respuesta de OpenAI: {responseString}");
                    return "Error al procesar las recomendaciones.";
                }
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

            decimal totalGastado = 0;
            foreach (var gasto in gastos)
            {
                promptBuilder.AppendLine($"- {gasto.Fecha.ToShortDateString()}: {gasto.Monto} en {gasto.Categoria ?? gasto.CategoriaPersonalizada} ({gasto.Cuenta})");
                totalGastado += gasto.Monto;
            }

            promptBuilder.AppendLine($"\nTotal gastado: {totalGastado}");
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