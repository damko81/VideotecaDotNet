using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VideotecaDotNet_VideotecaDotNetAPI.Data;
using VideotecaDotNet_VideotecaDotNetAPI.Dto;
using VideotecaDotNet_VideotecaDotNetAPI.Models;
using VideotecaDotNet_VideotecaDotNetAPI.Service;

namespace VideotecaDotNet_VideotecaDotNetAPI.Controllers
{
    [Route("api/FilesAPI")]
    [ApiController]
    public class FilesApiController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public FilesApiController(ApplicationDbContext db)
        {
            _db = db;
        }

        [EnableCors("BasicPolicy")]
        [HttpPost("Upload")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Upload(IFormFile file)
        {
            string path = "uploads/";

            if (file == null)
            {
                return BadRequest();
            }
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = path + file.FileName;

            using (var stream = System.IO.File.Create(path))
            {
                file.CopyTo(stream);
            }

            FilesApi fileApi = new()
            {
                Name = file.FileName,
                Path = path,
                Description = "",
                Size = file.Length
            };

            _db.FilesApi.Add(fileApi);
            _db.SaveChanges();

            return NoContent();
        }

    }

}
