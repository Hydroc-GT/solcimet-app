namespace Solcimet.Api.Entities
{
    public class Obras
    {
        public int Id_obra { get; set; }
        public string Codigo_obra { get; set; } = string.Empty;
        public string Nom_obra { get; set; } = string.Empty;
        public int Id_cliente { get; set; }
        public DateTime? Fecha_obra { get; set; }
        public DateTime Periodo { get; set; }
        public bool Facturado { get; set; } = false;
        public string Estado_obra { get; set; } = "activa";
        public DateTime Creado_en { get; set; }
        public DateTime Actualizado_en { get; set; }
    }
}
