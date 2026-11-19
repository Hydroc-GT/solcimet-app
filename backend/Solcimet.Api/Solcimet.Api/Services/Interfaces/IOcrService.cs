using Solcimet.Api.DTOs.OCR;

namespace Solcimet.Api.Services.Interfaces
{
    public interface IOcrService
    {
        Task<FacturaOcrDto> ProcesarFacturaAsync(IFormFile archivo);
    }
}
