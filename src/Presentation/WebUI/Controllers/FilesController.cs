using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    [Route("[controller]/[action]")]
    public class FilesController : BaseController
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FilesController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Get()
        {
            return Ok("files");
        }

        [HttpPost]
        public async Task<IActionResult> UploadAsync()
        {
            var reqForm = Request.Form;
            var file = Request.Form.Files.Count > 0 ? Request.Form.Files[0] : null;

            if (file != null && file.Length > 0)
            {

                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = file.FileName;
                var extension = fileName.Substring(fileName.LastIndexOf(".", StringComparison.Ordinal)).ToLower();
                var uniqueFileName = Guid.NewGuid().ToString("N") + extension;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                return Ok(new
                {
                    Url = Path.Combine($"{Request.Scheme}://{Request.Host.Value}", "uploads", uniqueFileName)
                });
            }
            return BadRequest();
        }
    }
}
