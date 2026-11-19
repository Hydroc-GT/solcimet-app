    using Solcimet.Api.DTOs.OCR;
    using System.Net.Http.Json;
    using System.Text.Json;
    using System.Text.RegularExpressions;

    namespace Solcimet.Api.Services.LLM

    {
        public class GeminiService : IGeminiService
        {
            private readonly HttpClient _http;
            private readonly ILogger<GeminiService> _logger;
            private readonly string _apiKey;
            private readonly string _model;

            private sealed record GeminiFactura(
                string? Ncf,
                decimal? Subtotal,
                decimal? Itbis,
                decimal? Total
                );

            private sealed record GeminiRespuesta(
                List<GeminiCandidate>? Candidates
                );

            private sealed record GeminiCandidate(
                GeminiContent? Content
                );

            private sealed record GeminiContent(
                List<GeminiPart>? Parts
                );
            private sealed record GeminiPart(
                string? Text
                );


            public GeminiService(
                HttpClient http,
                IConfiguration config,
                ILogger<GeminiService> logger
               )
            {
                _http = http;
                _logger = logger;

                _apiKey = config["Gemini:ApiKey"]
                    ?? throw new InvalidOperationException("Falta configurar la api key");

                _model = config["Gemini:Model"]
                    ?? "gemini-3.5-flash-lite";
            }
            public async Task<FacturaOcrDto?> EstructurarAsync(string textoOcr)
            {
                if (string.IsNullOrEmpty(textoOcr))
                {
                    _logger.LogWarning("El texto OCR esta vacio");

                    return null;
                }

                var prompt = $"""
                            Analiza el siguiente texto extraído de una factura.

                Identifica los siguientes campos:
                - Ncf
                - Subtotal
                - Itbis
                - Total

                Reglas:
                - Devuelve exclusivamente un JSON válido.
                - Si un campo no aparece, utiliza null.
                - Los valores monetarios deben ser números decimales.
                - No inventes información.
                - No confundas el número de factura con el NCF.

                Texto de la factura:
                {textoOcr}
                """;

                var contenido = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new { text  = prompt }
                            }
                        }
                    }
                };

                var url = $"https://generativelanguage.googleapis.com/v1beta/models/{_model}:generateContent?key={_apiKey}";

            var response = await _http.PostAsJsonAsync(url, contenido);

            var jsonRespuesta = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(
                    "Error de Gemini: {StatusCode} - {Respuesta}",
                    response.StatusCode,
                    jsonRespuesta
                );

                throw new HttpRequestException(
                    $"Gemini respondió {(int)response.StatusCode}: {jsonRespuesta}"
                );
            }

            var resultado = JsonSerializer.Deserialize<GeminiRespuesta>(
                    jsonRespuesta,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                
                );

                var textoJson = resultado?
                    .Candidates?
                    .FirstOrDefault()?
                    .Content?
                    .Parts?
                    .FirstOrDefault()?
                    .Text;

                if (string.IsNullOrWhiteSpace(textoJson))
                    return null;
            textoJson = textoJson.Trim();

            if (textoJson.StartsWith("```"))
            {
                textoJson = Regex.Replace(
                    textoJson,
                    @"^```(?:json)?\s*|\s*```$",
                    "",
                    RegexOptions.IgnoreCase
                ).Trim();
            }

            var factura = JsonSerializer.Deserialize<GeminiFactura>(
                    textoJson,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );


                if (factura == null)
                {
                    return null;
                }
                else
                {
                    return new FacturaOcrDto
                {
                    TextoExtraido = textoOcr,
                    Ncf = factura.Ncf,
                    Subtotal = factura.Subtotal,
                    Itbis = factura.Itbis,
                    Total = factura.Total
                };

                
                }
                


            }

       


        }
        
    }

