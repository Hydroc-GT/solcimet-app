namespace Solcimet.Api.Entities
{
    public class Terceros
    {
        public int Id_tercero { get; set; }
        public string Nom_legal { get; set; } = string.Empty;
        public string? Rnc_tercero { get; set; }
        public string Tipo_tercero { get; set; } = string.Empty;
        public bool Activo { get; set; } = true;
        public DateTime Creado_en { get; set; }
        public DateTime Actualizado_en { get; set; }
    }
}
