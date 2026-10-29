namespace Solcimet.Api.Entities
{
    public class Facturas
    {
        public long Id_factura { get; set; }
        public DateTime? Fecha_emision { get; set; }
        public string? Ncf { get; set; }
        public decimal? Subtotal { get; set; }
        public decimal? Itbis { get; set; }
        public decimal? Total { get; set; }
        public string Moneda { get; set; } = "DOP";
        public string Estado_factura { get; set; } = "borrador";
        public DateTime Fecha_registro { get; set; } //mapear en dbcontext
        public int? Id_tercero { get; set; }
        public int? Id_obra { get; set; }
        public int? Id_responsable { get; set; }
        public int Id_usuario { get; set; }
        public DateTime Actualizado_en { get; set; } //mapear en dbcontext
    }
}
