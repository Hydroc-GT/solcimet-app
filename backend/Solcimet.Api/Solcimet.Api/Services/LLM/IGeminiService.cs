using Solcimet.Api.DTOs.OCR;

namespace Solcimet.Api.Services.LLM
{
    public interface IGeminiService
    {
        Task<FacturaOcrDto?> EstructurarAsync(string textoOcr);
    }
}
