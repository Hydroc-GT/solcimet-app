
using Docnet.Core;
using Docnet.Core.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SkiaSharp;
using Solcimet.Api.DTOs.OCR;
using Solcimet.Api.Services.Interfaces;
using Solcimet.Api.Services.LLM;
using System.Runtime.InteropServices;
using System.Text;
using Tesseract;


namespace Solcimet.Api.Services
{
    public class OcrService : IOcrService
    {
        private readonly IWebHostEnvironment _environment;

        private readonly IGeminiService _gemini;

        public OcrService(IWebHostEnvironment environment, IGeminiService gemini)
        {
            _environment = environment;
            _gemini = gemini;
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

            var textoExtraido = textoCompleto.ToString();

            var resultado = await _gemini.EstructurarAsync(textoExtraido);

            return resultado ?? new FacturaOcrDto
            {
                TextoExtraido = textoExtraido
            };

        }

     
   

    }
}