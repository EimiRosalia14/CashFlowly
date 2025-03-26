using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CashFlowly.Core.Application.DTOs.Gastos;
using CashFlowly.Core.Application.DTOs.Metas;
using CashFlowly.Core.Application.Interfaces.Services;
using CashFlowly.Core.Domain.Entities;
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

        public async Task<string> GetGoalsRecommendationsAsync(int usuarioId)
        {
            _logger.LogInformation($"Iniciando obtención de recomendaciones para metas del usuario {usuarioId}.");
            try
            {
                _logger.LogInformation($"Obteniendo metas para usuario {usuarioId}.");
                var metas = await _metaService.ObtenerMetasPorUsuarioAsync(usuarioId);
                _logger.LogInformation($"Metas obtenidas para usuario {usuarioId}. Cantidad: {metas?.Count ?? 0}");

                if (metas == null || !metas.Any())
                {
                    _logger.LogWarning($"No se encontraron metas para el usuario {usuarioId}.");
                    return "No se encontraron metas para generar recomendaciones.";
                }

                var prompt = GeneratePrompt(metas);
                _logger.LogDebug($"Prompt generado: {prompt}");

                var requestData = new
                {
                    model = "gpt-4o-mini",
                    messages = new[] { new { role = "user", content = prompt } }
                };

                var json = JsonSerializer.Serialize(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var httpClient = _httpClientFactory.CreateClient("OpenAIClient");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _configuration["OpenAI:ApiKey"]);

                _logger.LogInformation("Enviando petición a la API de OpenAI.");
                var response = await httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);

                var responseString = await response.Content.ReadAsStringAsync();

                _logger.LogInformation($"Respuesta de OpenAI: {response.StatusCode}");
                _logger.LogDebug($"Cuerpo de respuesta de OpenAI: {responseString}");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Error en la API de OpenAI: {response.StatusCode} - {responseString}");
                    return $"Error al obtener recomendaciones. Código de estado: {response.StatusCode}. Respuesta: {responseString}";
                }

                try
                {
                    var responseObject = JsonSerializer.Deserialize<OpenAIResponse>(responseString);
                    var recommendation = responseObject?.choices?.FirstOrDefault()?.message?.content;

                    if (recommendation == null)
                    {
                        _logger.LogWarning("No se encontraron recomendaciones en la respuesta de OpenAI.");
                        return "No se encontraron recomendaciones en la respuesta.";
                    }

                    _logger.LogInformation("Recomendaciones obtenidas de OpenAI.");
                    return recommendation;
                }
                catch (JsonException ex)
                {
                    _logger.LogError(ex, $"Error al deserializar la respuesta de OpenAI: {responseString}");
                    return $"Error al procesar las recomendaciones. Respuesta: {responseString}";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener recomendaciones de OpenAI.");
                return $"Ocurrió un error al obtener las recomendaciones: {ex.Message}";
            }
        }

        private string GeneratePrompt(List<MostrarMetaDto> metas)
        {
            var promptBuilder = new StringBuilder();
            promptBuilder.AppendLine("Analiza las siguientes metas financieras y proporciona recomendaciones para lograr completarlas:");

            foreach (var meta in metas)
            {
                promptBuilder.AppendLine($"- Meta: {meta.Nombre}");
                promptBuilder.AppendLine($"  Objetivo: {meta.Objetivo:C}");
                promptBuilder.AppendLine($"  Progreso actual: {meta.ProgresoActual:C}");
                promptBuilder.AppendLine($"  Fecha propuesta: {meta.FechaPropuesta.ToShortDateString()}");

                decimal porcentajeCompletado = (meta.ProgresoActual / meta.Objetivo) * 100;
                promptBuilder.AppendLine($"  Porcentaje completado: {porcentajeCompletado:F2}%");

                // Calcula el tiempo restante estimado (en días)
                TimeSpan tiempoRestante = meta.FechaPropuesta - DateTime.Now;
                int diasRestantes = tiempoRestante.Days;

                if (diasRestantes > 0)
                {
                    promptBuilder.AppendLine($"  Tiempo restante estimado: {diasRestantes} días");

                    // Calcula cuánto se necesita ahorrar por día para alcanzar la meta
                    decimal ahorroDiarioNecesario = (meta.Objetivo - meta.ProgresoActual) / diasRestantes;
                    promptBuilder.AppendLine($"  Ahorro diario necesario: {ahorroDiarioNecesario:C}");
                }
                else
                {
                    promptBuilder.AppendLine("  Tiempo restante: La fecha propuesta ya ha pasado o es hoy.");
                    promptBuilder.AppendLine("  Ahorro diario necesario: No aplicable.");
                }

                promptBuilder.AppendLine(); // Espacio entre metas
            }

            promptBuilder.AppendLine("Proporciona recomendaciones concisas y prácticas para alcanzar cada meta, considerando el progreso actual, el tiempo restante y el ahorro diario necesario.");
            return promptBuilder.ToString();
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
}
