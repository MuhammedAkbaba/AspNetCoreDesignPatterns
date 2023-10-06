using DinkToPdf;
using DinkToPdf.Contracts;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.Extensions.Primitives;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Text;

namespace WebAp.Command.Commands
{
    /// <summary>
    /// UML deki Receiver
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PdfFile<T>
    {
        /// <summary>
        /// pdf convert etmek için kullandığımız kütüphane Html den pdf dönüştürme yapıyor 
        /// bu yüzden bizde data mızı html  olarak tasarlayacağız.
        /// kütüphane : DinkToPdf
        /// </summary>
        public readonly List<T> _list;
        public readonly HttpContext _httpContext;

        public PdfFile(HttpContext httpContext, List<T> list)
        {
            _httpContext = httpContext;
            _list = list;
        }

        public string FileName => $"{typeof(T).Name}.pdf";
        public string FileType => "application/octet-stream";

        public MemoryStream Create()
        {
            var type = typeof(T);
            var sb = new StringBuilder();
            sb.Append($@"<html>
                           <head></head>
                            <body>
                                <div><h1>{type.Name}</h1></div>
                                  <table class='table table-striped' aling='center'>");

                                    sb.Append("<tr>");
                                    type.GetProperties().ToList().ForEach(x =>
                                    {
                                        sb.Append($"<th>{x.Name}</th>");
                                    });
                                    sb.Append("</tr>");

                                    _list.ForEach(x =>
                                    {
                                        var values = type.GetProperties().ToList().Select(propinfo => propinfo.GetValue(x, null)).ToList();
                                     
                                        sb.Append("<tr>");
                                        values.ForEach(x =>
                                        {
                                            sb.Append($"<td>{x}</td>");
                                        });

                                        sb.Append("</tr>");
                                    });
            sb.Append("</table></body></html>");


            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = DinkToPdf.Orientation.Portrait,///dikey
                PaperSize = DinkToPdf.PaperKind.A4,
            },
                Objects = {
                    new ObjectSettings() {
                        PagesCount = true,
                        HtmlContent =sb.ToString(),
                        WebSettings = { DefaultEncoding = "utf-8",UserStyleSheet=Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/lib/bootstrap/dist/css/bootstrap.css") },
                        HeaderSettings = { FontSize = 9, Right = "Page [page] of [toPage]", Line = true, Spacing = 2.812 }
                    }
                }
            };


            var converter = _httpContext.RequestServices.GetRequiredService<IConverter>();

            //var _converter = new SynchronizedConverter(new PdfTools());
            //MemoryStream pdfMS = new(_converter.Convert(doc));

            MemoryStream pdfMemory = new(converter.Convert(doc));

            return pdfMemory;

        }
    }
}
