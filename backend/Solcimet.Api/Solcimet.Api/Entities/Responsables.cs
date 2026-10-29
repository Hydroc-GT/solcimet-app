namespace Solcimet.Api.Entities
{
    public class Responsables
    {
        public int Id_responsable { get; set; }
        public string Nom_responsable { get; set; } = string.Empty;
        public int? Id_usuario { get; set; }
        public bool Activo { get; set; } = true;
        public DateTime Creado_en { get; set; }
        public DateTime Actualizado_en { get; set; }
    }
}
