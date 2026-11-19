namespace Solcimet.Api.DTOs.OCR
{
    public class FacturaOcrDto
    {
        public string TextoExtraido { get; set; } = string.Empty;
        public string? Ncf { get; set; }
        public decimal? Subtotal { get; set; }
        public decimal? Itbis { get; set; }
        public decimal? Total { get; set; }
    }
}
