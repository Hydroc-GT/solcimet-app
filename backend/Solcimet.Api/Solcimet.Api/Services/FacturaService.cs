using Microsoft.EntityFrameworkCore;
using Solcimet.Api.Data;
using Solcimet.Api.DTOs.Facturas;
using Solcimet.Api.Entities;
using Solcimet.Api.Services.Interfaces;


namespace Solcimet.Api.Services
{
    public class FacturaService : IFacturaService
   
    {
        private readonly DatabaseContext _context;

        public FacturaService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<FacturaRespuestaDto>> ObtenerFacturasAsync()
        {
            return await _context.Factura
                .AsNoTracking()
                .Select(f => new FacturaRespuestaDto
                {
                    Id_factura = f.Id_factura,
                    Fecha_emision = f.Fecha_emision,
                    Ncf = f.Ncf,
                    Subtotal = f.Subtotal,
                    Itbis = f.Itbis,
                    Total = f.Total,
                    Moneda = f.Moneda,
                    Id_tercero = f.Id_tercero,
                    Id_obra = f.Id_obra,
                    Id_responsable = f.Id_responsable,
                    Nom_tercero = f.Tercero != null
                        ? f.Tercero.Nom_legal
                        : null,
                    Nom_obra = f.Obra != null
                        ? f.Obra.Nom_obra
                        : null,
                    Nom_responsable = f.Responsable != null
                        ? f.Responsable.Nom_responsable
                        : null
                })
                .ToListAsync();
        }
        public async Task<FacturaRespuestaDto?> ObtenerFacturaPorIdAsync(long id)
        {
            return await _context.Factura
                .AsNoTracking()
                .Where(f => f.Id_factura == id)
                .Select(f => new FacturaRespuestaDto
                {
                    Id_factura = f.Id_factura,
                    Fecha_emision = f.Fecha_emision,
                    Ncf = f.Ncf,
                    Subtotal = f.Subtotal,
                    Itbis = f.Itbis,
                    Total = f.Total,
                    Moneda = f.Moneda,
                    Id_tercero = f.Id_tercero,
                    Id_obra = f.Id_obra,
                    Id_responsable = f.Id_responsable,
                    Nom_tercero = f.Tercero != null
                        ? f.Tercero.Nom_legal
                        : null,
                    Nom_obra = f.Obra != null
                        ? f.Obra.Nom_obra
                        : null,
                    Nom_responsable = f.Responsable != null
                        ? f.Responsable.Nom_responsable
                        : null
                })
                .FirstOrDefaultAsync();
        }
    }
}
