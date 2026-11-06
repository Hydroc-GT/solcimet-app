using Microsoft.EntityFrameworkCore;
using Solcimet.Api.Data;
using Solcimet.Api.DTOs.Facturas;
using Solcimet.Api.Entities;
using Solcimet.Api.Services.Interfaces;
using System.Security.Claims;


namespace Solcimet.Api.Services
{
    public class FacturaService : IFacturaService
   
    {
        private readonly DatabaseContext _context;

    
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FacturaService(
            DatabaseContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<FacturaRespuestaDto>> ObtenerFacturasAsync()
        {

            var idUsuario = _httpContextAccessor.HttpContext?
                .User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(idUsuario))
                throw new UnauthorizedAccessException("Usuario no autenticado.");

            return await _context.Factura
                .AsNoTracking()
                .Where(f => f.Id_usuario == long.Parse(idUsuario))
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
                        : null,
                    Estado_factura = f.Estado_factura
        
                })
                .ToListAsync();
        }
        public async Task<FacturaRespuestaDto?> ObtenerFacturaPorIdAsync(long id)
        {
            var idUsuario = _httpContextAccessor.HttpContext?
                .User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(idUsuario))
                throw new UnauthorizedAccessException("Usuario no autenticado.");

            return await _context.Factura
                .AsNoTracking()
                .Where(f => f.Id_usuario == long.Parse(idUsuario))
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
                        : null,
                    Estado_factura = f.Estado_factura
              
                })
                .FirstOrDefaultAsync();
        }
        public async Task<FacturaRespuestaDto> CrearFacturaAsync(CrearFacturaDto dto)
        {
            var idUsuario = _httpContextAccessor.HttpContext?
                .User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(idUsuario))
                throw new UnauthorizedAccessException("Usuario no autenticado.");

            var factura = new Facturas
            { 
                Ncf = dto.Ncf,
                Subtotal = dto.Subtotal,
                Itbis = dto.Itbis,
                Total = dto.Total,
                Moneda = dto.Moneda,
                Id_tercero = dto.Id_tercero,
                Id_obra = dto.Id_obra,
                Id_responsable = dto.Id_responsable,
                Id_usuario = Convert.ToInt32(idUsuario)
            };
            _context.Factura.Add(factura);

            await _context.SaveChangesAsync();

           var respuesta =  await _context.Factura
            .AsNoTracking()
            .Where(f => f.Id_factura == factura.Id_factura)
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
                    : null,
                Estado_factura = f.Estado_factura
            
            })
            .FirstAsync();

            return respuesta;


        }

        public async Task<bool> ActualizarFacturaAsync(long id, ActualizarFacturaDto dto)
        {
            var idUsuario = _httpContextAccessor.HttpContext?
                .User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(idUsuario))
                throw new UnauthorizedAccessException("Usuario no autenticado.");

            var factura = await _context.Factura
                 .FirstOrDefaultAsync(f => f.Id_factura == id);

                if(factura == null)
            {
                return false;
            }

            factura.Fecha_emision = dto.Fecha_emision;
            factura.Ncf = dto.Ncf;
            factura.Subtotal = dto.Subtotal;
            factura.Itbis = dto.Itbis;
            factura.Total = dto.Total;
            factura.Moneda = dto.Moneda;
            factura.Id_tercero = dto.Id_tercero;
            factura.Id_obra = dto.Id_obra;
            factura.Id_responsable = dto.Id_responsable;
            factura.Estado_factura = dto.Estado_factura;

            await _context.SaveChangesAsync();

            return true;

        }

        public async Task<bool>EliminarFacturaAsync(long id)
        {
            var idUsuario = _httpContextAccessor.HttpContext?
        .User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!long.TryParse(idUsuario, out var usuarioId))
                throw new UnauthorizedAccessException("Usuario no autenticado.");

            var factura = await _context.Factura
                .FirstOrDefaultAsync(f =>
                    f.Id_factura == id &&
                    f.Id_usuario == usuarioId);

            if (factura == null)
                return false;

            factura.Estado_factura = "anulada";

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
