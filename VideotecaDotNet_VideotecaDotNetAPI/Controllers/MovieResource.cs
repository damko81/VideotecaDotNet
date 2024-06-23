using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using VideotecaDotNet_VideotecaDotNetAPI.Data;
using VideotecaDotNet_VideotecaDotNetAPI.Dto;
using VideotecaDotNet_VideotecaDotNetAPI.Logging;

namespace VideotecaDotNet_VideotecaDotNetAPI.Controllers
{
    [Route("api/MovieAPI")]
    [ApiController]
    public class MovieResource : ControllerBase
    {
        private readonly ILogging _logger;
    
        public MovieResource(ILogging logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<MovieDTO>> GetAllMovies()
        {
            _logger.Log("Getting all movies","");
            return Ok(MovieStore.movieList);
        }

        [HttpGet("{id:long}",Name = "GetMovie")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<MovieDTO> GetMovieById(long id)
        {
            if(id == 0)
            {
                _logger.Log("Get Movie Error with Id: " + id, "error");
                return BadRequest();
            }
            var movie = MovieStore.movieList.FirstOrDefault(u => u.Id == id);
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
            if (MovieStore.movieList.FirstOrDefault(u => u.Name.ToLower() == movieDTO.Name.ToLower() && u.Disc.ToLower() == movieDTO.Disc.ToLower()) != null)
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
            movieDTO.Id = MovieStore.movieList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
            MovieStore.movieList.Add(movieDTO);

            return CreatedAtRoute("GetMovie",new { id = movieDTO.Id },movieDTO);
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
            var movie = MovieStore.movieList.FirstOrDefault(u => u.Id == id);
            if (movie == null)
            {
                return NotFound();
            }
            MovieStore.movieList.Remove(movie);

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
            var movie = MovieStore.movieList.FirstOrDefault(u => u.Id == id);
            movie.Name = movieDTO.Name;
            movie.Disc = movieDTO.Disc;

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
            var movie = MovieStore.movieList.FirstOrDefault(u => u.Id == id);

            if (movie == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(movie,ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return NoContent();

        }

    }
}
