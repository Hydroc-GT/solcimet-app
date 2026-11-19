using Microsoft.AspNetCore.Mvc;
using Solcimet.Api.Services.Interfaces;


namespace Solcimet.Api.Controllers
{

    

    [ApiController]
    [Route("api/[controller]")]
    public class FacturasController: ControllerBase
    {
        private readonly IFacturaService _facturaService;
        private readonly IOcrService _ocrService;

        public FacturasController(
            IFacturaService facturaService,
            IOcrService ocrService
            )
        {
            _facturaService = facturaService;
            _ocrService = ocrService;


        }

        [HttpPost("procesar-ocr")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult>ProcesarOcr(
            IFormFile archivo)
        {
            try
            {
                var resultado = await _ocrService.ProcesarFacturaAsync(archivo);
                return Ok(resultado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

    }
}