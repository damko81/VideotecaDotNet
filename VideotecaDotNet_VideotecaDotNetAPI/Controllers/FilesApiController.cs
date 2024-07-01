using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IO;
using System.Reflection.Metadata;
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
          
            var filesApi = _db.FilesApi.Where(fields => fields.Kind.Equals("upload"))
                                       .GroupBy(fields => fields.Name)
                                       .Select(fields => fields
                                                    .OrderByDescending(x => x.Id)
                                                    .Select(x => new
                                                    {
                                                        Id = x.Id,
                                                        Name = x.Name,
                                                        Path = x.Path,
                                                        Kind = x.Kind,
                                                        Description = x.Description,
                                                        Size = x.Size,
                                                        Data = x.Data,
                                                        CreatedDate = x.CreatedDate
                                                    })
                                                    .First()
                                              )
                                       .ToList();

            return Ok(filesApi);
        }

        [EnableCors("BasicPolicy")]
        [HttpGet("Download")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<FilesApiDTO>> Download()
        {
            var filesApi = _db.FilesApi.Where(f => f.Kind.Equals("download"))
                                       .OrderByDescending(f => f.Id)
                                       .Take(1)
                                       .ToList();

            return Ok(filesApi);
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

            var filesApi = _db.FilesApi.Where(f => f.Name.Contains(username + '_') && f.Kind.Equals("upload"))
                                       .OrderByDescending(f => f.Id)
                                       .Take(1)
                                       .ToList();

            return Ok(filesApi);
        }

        [EnableCors("BasicPolicy")]
        [HttpPost("Upload")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Upload(IFormFile file)
        {
            string path = "upload/";

            if (file == null || file.Length == 0)
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

            using (var memorySteam = new MemoryStream())
            {
                file.CopyTo(memorySteam);

                FilesApi fileApi = new()
                {
                    Name = file.FileName,
                    Path = path,
                    Description = "",
                    Kind = "upload",
                    Size = file.Length,
                    Data = memorySteam.ToArray()
                };

                _db.FilesApi.Add(fileApi);
                _db.SaveChanges();

            }

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

            FilesApi fileApi = new()
            {
                Name = "Filmi.xml",
                Path = path,
                Description = "",
                Kind = "download",
                Size = 0,
                Data = new byte[] { 0x20 }
            };

            _db.FilesApi.Add(fileApi);
            _db.SaveChanges();

            return NoContent();
        }

        [EnableCors("BasicPolicy")]
        [HttpDelete("{id:long}", Name = "Delete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Delete(long id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var filesApi = _db.FilesApi.FirstOrDefault(f => f.Id == id);
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
