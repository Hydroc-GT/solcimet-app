namespace Solcimet.Api.DTOs.Facturas
{
    public class ActualizarFacturaDto
    {
        public DateTime? Fecha_emision { get; set; }
        public string? Ncf { get; set; }
        public decimal? Subtotal { get; set; }
        public decimal? Itbis { get; set; }
        public decimal? Total { get; set; }
        public string Moneda { get; set; } = "DOP";
        public int? Id_tercero { get; set; }
        public int? Id_obra { get; set; }
        public int? Id_responsable { get; set; }
    }
}
