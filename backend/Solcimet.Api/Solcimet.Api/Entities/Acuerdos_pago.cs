namespace Solcimet.Api.Entities
{
    public class Acuerdos_pago
    {
        public int Id_obra { get; set; }
        public int Id_tercero { get; set; }
        public decimal Monto_acordado { get; set; }
        public bool Incluye_itbis { get; set; } = false;
        public DateTime Creado_en { get; set; }
        public DateTime Actualizado_en { get; set; }
    }
}
