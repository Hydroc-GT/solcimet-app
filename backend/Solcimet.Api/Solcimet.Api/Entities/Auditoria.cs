namespace Solcimet.Api.Entities
{
    public class Auditoria
    {
        public long Id_auditoria { get; set; }
        public int Id_usuario { get; set; }
        public string Accion { get; set; } = string.Empty;
        public string Entidad { get; set; } = string.Empty;
        public string Id_entidad { get; set; } = string.Empty;
        public string Cambios { get; set; } = string.Empty;
        public DateTime Fecha_hora { get; set; } //mapear en dbcontext
    }
}
