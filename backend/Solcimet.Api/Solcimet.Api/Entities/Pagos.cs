namespace Solcimet.Api.Entities
{
    public class Pagos
    {
        public long Id_pago { get; set; }
        public int Id_obra { get; set; }
        public int Id_tercero { get; set; }
        public DateTime Fecha_pago { get; set; }
        public decimal Monto_pagado { get; set; }
        public string? Referencia { get; set; }
        public DateTime Creado_en { get; set; }
    }
}
