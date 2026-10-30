namespace Solcimet.Api.DTOs.Facturas
{
    public class FacturaRespuestaDto
    {
        public long Id_factura { get; set; }
        public DateTime? Fecha_emision { get; set; }
        public string? Ncf { get; set; }
        public decimal? Subtotal { get; set; }
        public decimal? Itbis { get; set; }
        public decimal? Total { get; set; }
        public string Moneda { get; set; } = "DOP";
        public string?Nom_obra { get; set;}
        public string? Nom_tercero { get; set; }
        public string? Nom_responsable { get; set; }

        public int? Id_tercero { get; set; }
        public int? Id_obra { get; set; }
        public int? Id_responsable { get; set; }
        public DateTime Fecha_registro { get; set; }
    }
}
