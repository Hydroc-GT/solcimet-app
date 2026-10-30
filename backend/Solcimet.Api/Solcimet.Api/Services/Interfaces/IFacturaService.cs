using Solcimet.Api.DTOs.Facturas;

namespace Solcimet.Api.Services.Interfaces
{
    public interface IFacturaService
    {
        Task<List<FacturaRespuestaDto>> ObtenerFacturasAsync();
        Task<FacturaRespuestaDto> ObtenerFacturaPorIdAsync(long id);
        Task<FacturaRespuestaDto> CrearFacturaAsync(CrearFacturaDto dto);
        Task<bool> ActualizarFacturaAsync(long id, ActualizarFacturaDto dto);
        Task<bool> EliminarFacturaAsync(long id);
    }
}
