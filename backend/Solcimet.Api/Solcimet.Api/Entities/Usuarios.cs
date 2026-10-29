namespace Solcimet.Api.Entities
{
    public class Usuarios
    {
        public int Id_usuario { get; set; }
        public string Nom_usuario { get; set; } = string.Empty;
        public string Email_usuario { get; set; } = string.Empty;
        public short Id_rol { get; set; }
        public bool Activo { get; set; } = true;
        public DateTime Creado_en { get; set; }
        public DateTime Actualizado_en { get; set; }
    }
}
