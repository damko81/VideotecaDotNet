using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VideotecaDotNet_VideotecaDotNetAPI.Data;
using VideotecaDotNet_VideotecaDotNetAPI.Dto;
using VideotecaDotNet_VideotecaDotNetAPI.Models;
using VideotecaDotNet_VideotecaDotNetAPI.Service;

namespace VideotecaDotNet_VideotecaDotNetAPI.Controllers
{
    [Route("api/MovieAPI")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
       
        public MovieController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet(Name = "GetAllMovies")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<MovieDTO>> GetAllMovies()
        {
            return Ok(_db.Movies.ToList());
        }

        [HttpGet("{id:long}",Name = "GetMovie")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<MovieDTO> GetMovieById(long id)
        {
            if(id == 0)
            {
                return BadRequest();
            }
            var movie = _db.Movies.FirstOrDefault(u => u.Id == id);
            if(movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<MovieDTO> AddMovie([FromBody]MovieDTO movieDTO)
        {
            if (_db.Movies.FirstOrDefault(u => u.Name.ToLower() == movieDTO.Name.ToLower() && u.Disc.ToLower() == movieDTO.Disc.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError","Movie already Exists!");
                return BadRequest(ModelState);
            }
            if (movieDTO == null)
            {
                return BadRequest(movieDTO);
            }
            if (movieDTO.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            Movie movie = new()
            {
                Id = movieDTO.Id,
                Disc = movieDTO.Disc,
                Name = movieDTO.Name,
                NameFromDisc = movieDTO.NameFromDisc,
                Genre = movieDTO.Genre,
                Rating = movieDTO.Rating,
                Description = movieDTO.Description,
                Stars = movieDTO.Stars,
                Infobar = movieDTO.Infobar,
                Director = movieDTO.Director,
                Duration = movieDTO.Duration,
                Storyline = movieDTO.Storyline,
                ReleaseDate = movieDTO.ReleaseDate,
                Url = movieDTO.Url,
                ImageSrc = movieDTO.ImageSrc,
            };

            _db.Movies.Add(movie);
            _db.SaveChanges();

            return CreatedAtRoute("GetMovie",new { id = movieDTO.Id },movieDTO);
        }

        [HttpPost("{disc}", Name = "LoadMovies")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult LoadMovies(string disc)
        {
            string discTmp = disc.Replace("!","\\");
            List<Movie> movies = BusinessService.LoadMovies(discTmp);
            foreach (Movie movie in movies)
            {
                _db.Movies.Add(movie);
                _db.SaveChanges();
            }

            return NoContent();
        }

        [HttpDelete("{disc}", Name = "DeleteMovieByDisc")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteMovieByDisc(string disc)
        {
            string discTmp = disc.Replace("!", "\\");
            var ids = from m in _db.Movies where m.Disc == discTmp select m.Id;
                
            if (ids == null)
            {
                return NotFound();
            }

            foreach (long id in ids)
            {
                DeleteMovie(id);
            }

            return NoContent();
        }

        [HttpDelete("{id:long}", Name = "DeleteMovie")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteMovie(long id)
        {
            if(id == 0)
            {
                return BadRequest();
            }
            var movie = _db.Movies.FirstOrDefault(u => u.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            _db.Movies.Remove(movie);
            _db.SaveChanges();

            return NoContent();
        }

        [HttpPut("{id:long}", Name = "UpdateMovie")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<MovieDTO> UpdateMovie([FromBody] MovieDTO movieDTO, long id)
        {
            if (movieDTO == null || id != movieDTO.Id)
            {
                return BadRequest();
            }

            Movie model = new ()
            {
                Id = movieDTO.Id,
                Disc = movieDTO.Disc,
                Name = movieDTO.Name,
                NameFromDisc = movieDTO.NameFromDisc,
                Genre = movieDTO.Genre,
                Rating = movieDTO.Rating,
                Description = movieDTO.Description,
                Stars = movieDTO.Stars,
                Infobar = movieDTO.Infobar,
                Director = movieDTO.Director,
                Duration = movieDTO.Duration,
                Storyline = movieDTO.Storyline,
                ReleaseDate = movieDTO.ReleaseDate,
                Url = movieDTO.Url,
                ImageSrc = movieDTO.ImageSrc,
            };

            _db.Movies.Update(model);
            _db.SaveChanges();

            return NoContent();

        }

        [HttpPatch("{id:long}", Name = "UpdatePartialMovie")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartialMovie(JsonPatchDocument<MovieDTO> patchDTO, long id)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            var movie = _db.Movies.AsNoTracking().FirstOrDefault(u => u.Id == id);

            if (movie == null)
            {
                return BadRequest();
            }

            MovieDTO movieDTO = new()
            {
                Id = movie.Id,
                Disc = movie.Disc,
                Name = movie.Name,
                NameFromDisc = movie.NameFromDisc,
                Genre = movie.Genre,
                Rating = movie.Rating,
                Description = movie.Description,
                Stars = movie.Stars,
                Infobar = movie.Infobar,
                Director = movie.Director,
                Duration = movie.Duration,
                Storyline = movie.Storyline,
                ReleaseDate = movie.ReleaseDate,
                Url = movie.Url,
                ImageSrc = movie.ImageSrc,
            };

            patchDTO.ApplyTo(movieDTO,ModelState);

            Movie model = new()
            {
                Id = movieDTO.Id,
                Disc = movieDTO.Disc,
                Name = movieDTO.Name,
                NameFromDisc = movieDTO.NameFromDisc,
                Genre = movieDTO.Genre,
                Rating = movieDTO.Rating,
                Description = movieDTO.Description,
                Stars = movieDTO.Stars,
                Infobar = movieDTO.Infobar,
                Director = movieDTO.Director,
                Duration = movieDTO.Duration,
                Storyline = movieDTO.Storyline,
                ReleaseDate = movieDTO.ReleaseDate,
                Url = movieDTO.Url,
                ImageSrc = movieDTO.ImageSrc,
            };

            _db.Movies.Update(model);
            _db.SaveChanges();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return NoContent();

        }

    }
}
