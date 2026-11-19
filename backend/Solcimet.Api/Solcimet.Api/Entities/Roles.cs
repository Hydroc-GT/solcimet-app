namespace Solcimet.Api.Entities
{
    public class Roles
    {
        public short Id_rol { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nom_rol { get; set; } = string.Empty;
        public bool Activo { get; set; } = true;
        public DateTime Creado_en { get; set;}
        public DateTime Actualizado_en { get; set;}

    }
}
