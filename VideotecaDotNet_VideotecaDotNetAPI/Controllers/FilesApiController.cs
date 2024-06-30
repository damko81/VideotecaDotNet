using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
        [HttpGet(Name = "GetFiles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<FilesApiDTO>> GetFiles()
        {
            return Ok(_db.FilesApi.ToList());
        }

        [EnableCors("BasicPolicy")]
        [HttpGet("{username}", Name = "GetForLoginFiles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<FilesApiDTO>> GetForLoginFiles(string username)
        {
            if (username.IsNullOrEmpty())
            {
                return BadRequest();
            }
            var filesApi = from f in _db.FilesApi where f.Name.Contains(username + '_') select f;

            return Ok(filesApi.ToList());
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
                Description = "Upload",
                Size = file.Length
            };

            _db.FilesApi.Add(fileApi);
            _db.SaveChanges();

            return NoContent();
        }

        [EnableCors("BasicPolicy")]
        [HttpPost("Export")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Export()
        {
            string path = "download/";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = path + "Filmi.xml";

            using (var stream = System.IO.File.Create(path))
            {
                 // TODO: Load all movies to xml file.
            }

            return NoContent();
        }

        [EnableCors("BasicPolicy")]
        [HttpDelete("{name}", Name = "Delete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Delete(string name)
        {
            if (name.IsNullOrEmpty())
            {
                return BadRequest();
            }
            var filesApi = _db.FilesApi.FirstOrDefault(f => f.Name == name);
            if (filesApi == null)
            {
                return NotFound();
            }

            _db.FilesApi.Remove(filesApi);
            _db.SaveChanges();

            return NoContent();
        }


    }

}
