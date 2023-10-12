using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;
using WebAp.Command.Commands;
using WebApp.Command.Commands;
using WebApp.Command.Models;

namespace WebApp.Command.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppIdentityDbContext _context;


        public ProductController(AppIdentityDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }

        public async Task<IActionResult> CreateFile(int type)
        {

            var _productList = await _context.Products.ToListAsync();

            FileCreateInvoker fileCreateInvoker = new();

            EFileType eFileType = (EFileType)type;

            switch (eFileType)
            {
                case EFileType.Excel:

                    ExcelFile<Product> excelFile = new(_productList);

                    fileCreateInvoker.SetCommand(new CreateExcelTableActionCommand<Product>(excelFile));

                    //fileCreateInvoker.SetCommand(new CreateExcelTableActionCommand<Product>(new ExcelFile<Product>(_productList)));

                    break;
                case EFileType.Pdf:
                    PdfFile<Product> pdfFile = new(HttpContext, _productList);
                    fileCreateInvoker.SetCommand(new CreatePdfTableActionCommand<Product>(pdfFile));

                    //fileCreateInvoker.SetCommand(new CreatePdfTableActionCommand<Product>(new PdfFile<Product>(HttpContext, _productList)));

                    break;
                case EFileType.Json:
                    JsonFile<Product> jsonFile = new(_productList);
                    fileCreateInvoker.SetCommand(new CreateJsonTableActionCommand<Product>(jsonFile));
                    break;
            }

            return fileCreateInvoker.CreateFile();

        }


        public async Task<IActionResult> CreateFiles()
        {
            var _productList = await _context.Products.ToListAsync();

            ExcelFile<Product> excelFile = new(_productList);
            PdfFile<Product> pdfFile = new(HttpContext, _productList);


            FileCreateInvoker fileCreateInvoker = new();
            fileCreateInvoker.AddCommand(new CreateExcelTableActionCommand<Product>(excelFile));
            fileCreateInvoker.AddCommand(new CreatePdfTableActionCommand<Product>(pdfFile));


            var filesResult = fileCreateInvoker.CreateFiles();

            ////oluşan dosyaları zipleme işlemi yapma
            using (var zipMemoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(zipMemoryStream, ZipArchiveMode.Create))
                {
                    foreach (var item in filesResult)
                    {
                        var fileContent = item as FileContentResult;

                        var zipFile = archive.CreateEntry(fileContent.FileDownloadName);

                        using (var zipEntryStream = zipFile.Open())
                        {
                            await new MemoryStream(fileContent.FileContents).CopyToAsync(zipEntryStream);
                        }
                    }
                }


                return File(zipMemoryStream.ToArray(), "application/zip", "all.zip");
            }


        }

    }
}
