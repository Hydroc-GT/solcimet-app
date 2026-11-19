
using Solcimet.Api.DTOs.OCR;
using Solcimet.Api.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Tesseract;
using Docnet.Core;
using Docnet.Core.Models;
using SkiaSharp;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Solcimet.Api.Services
{
    public class OcrService : IOcrService
    {
        private readonly IWebHostEnvironment _environment;


        public OcrService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }


        private List<byte[]> ConvertirPdfaImagen(byte[] pdfBytes)
        {
            var imagenes = new List<byte[]>();

            using var documento = DocLib.Instance.GetDocReader(
                pdfBytes,
                new PageDimensions(1500, 2000)
                
                );
            int cantidadPaginas = documento.GetPageCount();
            for (int i = 0; i < cantidadPaginas; i++)
            {
                using var pagina = documento.GetPageReader(i);

                byte[] bytesImagen = pagina.GetImage();

                int ancho = pagina.GetPageWidth();
                int alto = pagina.GetPageHeight();

                using var bitmap = new SKBitmap(
                    new SKImageInfo(
                        ancho,
                        alto,
                        SKColorType.Bgra8888,
                        SKAlphaType.Unpremul
                        )
                    
                    );

                Marshal.Copy(
                    bytesImagen,
                    0,
                    bitmap.GetPixels(),
                    bytesImagen.Length
                    );

                using var imagen = SKImage.FromBitmap(bitmap);
                using var png = imagen.Encode(SKEncodedImageFormat.Png, 100);
                imagenes.Add(png.ToArray());
            }
            return imagenes;
        }


        public async Task<FacturaOcrDto> ProcesarFacturaAsync(IFormFile archivo)
        {
            if (archivo == null || archivo.Length == 0)
            {
                throw new ArgumentException("El archivo esta vacio");

            }

            var extensionesPermitidas = new[]
             {
                    ".jpg", ".png", ".jpeg", ".tif", ".pdf"
                };

            var extension = Path.GetExtension(archivo.FileName)
                    .ToLowerInvariant();

                if (!extensionesPermitidas.Contains(extension))
                {
                    throw new ArgumentException("Formato de archivo no permitido.");
                }

                var rutaTessdata = Path.Combine(
                    _environment.ContentRootPath,
                    "tessdata"
                  );

                using var engine = new TesseractEngine(
                    rutaTessdata,
                    "spa",
                    EngineMode.Default
                 );

            using var memoria = new MemoryStream();
            await archivo.CopyToAsync(memoria);

            byte[] archivoBytes = memoria.ToArray();

            var textoCompleto = new StringBuilder();

            if (extension == ".pdf")
            {
                var imagenes = ConvertirPdfaImagen(archivoBytes);

                foreach (var bytesImagen in imagenes)
                {
                    using var imagen = Pix.LoadFromMemory(bytesImagen);
                    using var pagina = engine.Process(imagen);

                    textoCompleto.AppendLine(pagina.GetText());
                }
            }
            else
            {
                using var imagen = Pix.LoadFromMemory(archivoBytes);
                using var pagina = engine.Process(imagen);  

                textoCompleto.AppendLine(pagina.GetText());
            }

            return ProcesarTexto(textoCompleto.ToString());

        }

        public FacturaOcrDto ProcesarTexto(string texto)
        {
            var dto = new FacturaOcrDto { TextoExtraido = texto };
            ExtraerCampos(texto, dto);
            return dto;
        }

        private static decimal? ParseMonto(string valor)
        {
            valor = valor.Trim().TrimEnd('.', ',');

            int lastDot = valor.LastIndexOf('.');
            int lastComma = valor.LastIndexOf(',');

            if (lastDot >= 0 && lastComma >= 0)
            {
                valor = lastDot > lastComma
                    ? valor.Replace(",", "")
                    : valor.Replace(".", "").Replace(",", ".");
            }
            else if (lastComma >= 0)
            {
                valor = Regex.IsMatch(valor, @",\d{2}$")
                    ? valor.Replace(",", ".")
                    : valor.Replace(",", "");
            }

            return decimal.TryParse(valor, NumberStyles.Number,
                                    CultureInfo.InvariantCulture, out var r) ? r : null;
        }
        private static readonly Regex MontoRx = new(
      @"(?<![\d.,])(\d{1,3}(?:[.,]\d{3})+[.,]\d{2}|\d+[.,]\d{2})(?!\d)",
      RegexOptions.Compiled);

        private static readonly Regex NcfValidoRx = new(
            @"^(B(01|02|03|04|11|12|13|14|15|16|17)\d{8}|E(31|32|33|34|41|43|44|45|46|47)\d{10})$",
            RegexOptions.Compiled);

        private void ExtraerCampos(string texto, FacturaOcrDto dto)
        {
            dto.Ncf = ExtraerNcf(texto);

            dto.Subtotal = Montos(texto, @"sub\s*-?\s*total|monto\s+gravado|base\s+imponible")
                .Cast<decimal?>().FirstOrDefault();

            dto.Itbis = Montos(texto, @"itbis")
                .Cast<decimal?>().FirstOrDefault();

            var totales = Montos(texto,
    @"valor\s+total|valor\s+facturado|\btotal\b(?:\s+a\s+pagar|\s+general)?|monto\s+total",
    excluir: @"itbis|pendiente|descuento");
            dto.Total = totales.Count > 0 ? totales.Max() : null;
        }

        private static string? ExtraerNcf(string texto)
        {
            // 1) con etiqueta NCF (ignora ceros de relleno), 2) suelto en el texto
            var m = Regex.Match(texto, @"NCF\s*:?\s*0*([A-Z0-9]{11,13})", RegexOptions.IgnoreCase);
            var candidato = m.Success
                ? m.Groups[1].Value
                : Regex.Match(texto, @"\b([BE]\d{10,12})\b", RegexOptions.IgnoreCase)
                       .Groups[1].Value;

            candidato = candidato.ToUpperInvariant();
            return NcfValidoRx.IsMatch(candidato) ? candidato : null;
        }

        private static List<decimal> Montos(string texto, string etiqueta, string? excluir = null)
        {
            var res = new List<decimal>();
            var lineas = texto.Split('\n').Select(l => l.Trim()).ToArray();
            var rx = new Regex(etiqueta, RegexOptions.IgnoreCase);

            for (int i = 0; i < lineas.Length; i++)
            {
                var m = rx.Match(lineas[i]);
                if (!m.Success) continue;
                if (excluir != null && Regex.IsMatch(lineas[i], excluir, RegexOptions.IgnoreCase))
                    continue;

                var resto = lineas[i][(m.Index + m.Length)..];
                var montos = MontoRx.Matches(resto);

                // si no hay monto en la misma línea, busca en la siguiente
                if (montos.Count == 0 && i + 1 < lineas.Length)
                    montos = MontoRx.Matches(lineas[i + 1]);

                // el monto de más a la derecha es la columna "Valor"
                if (montos.Count > 0 && ParseMonto(montos[^1].Value) is decimal d)
                    res.Add(d);
            }
            return res;
        }

        private static decimal? Primero(List<decimal> l) => l.Count > 0 ? l[0] : null;

    }
}