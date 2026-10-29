namespace Solcimet.Api.Entities
{
    public class Factura_archivos
    {
        public long Id_factura { get; set; }
        public int Id_archivo { get; set; }
        public string Ruta_archivo { get; set; } = string.Empty;
        public string Tipo_mime { get; set; } = string.Empty;
        public DateTime Creado_en { get; set; }
    }
}
