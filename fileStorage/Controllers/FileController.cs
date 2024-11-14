using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace FileStorageApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly string _storagePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

        public FileController()
        {
            
            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }
        }


        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadFile([FromForm] FileUploadModel model)
        {
            if (model.File == null || model.File.Length == 0)
                return BadRequest("Nenhum arquivo enviado.");

          
            var virtualDir = "files"; // Substitua pelo alias configurado no IIS
            var fileName = Path.GetFileName(model.File.FileName);

          
            var filePath = Path.Combine(@"C:\inetpub\files", fileName);

            
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.File.CopyToAsync(stream);
            }

        // Gera a URL de download direto para o arquivo
             
            var fileUrl = $"http://localhost:5175/{virtualDir}/{fileName}";

            return Ok(new { FileName = fileName, FileUrl = fileUrl });
        }


        [HttpGet("download/{fileName}")]
        public IActionResult GetDownloadLink(string fileName)
        {
            // Monta o caminho físico para verificar se o arquivo existe
            var filePath = Path.Combine(@"C:\inetpub\files", fileName);

            if (!System.IO.File.Exists(filePath))
                return NotFound("Arquivo não encontrado.");

            // Constrói a URL pública do arquivo, usando o alias do diretório virtual
            var virtualDir = "files"; // Altere para o alias configurado no IIS
            var fileUrl = $"http://localhost:5175/{virtualDir}/{fileName}";

            // Retorna a URL como resposta
            return Ok(new { FileName = fileName, DownloadUrl = fileUrl });
        }

    }
}
