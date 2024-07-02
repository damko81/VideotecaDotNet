
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VideotecaDotNet_VideotecaDotNetAPI.Data;
using VideotecaDotNet_VideotecaDotNetAPI.Dto;
using VideotecaDotNet_VideotecaDotNetAPI.Models;
using VideotecaDotNet_VideotecaDotNetAPI.Service;

namespace VideotecaDotNet_VideotecaDotNetAPI.Controllers
{
    [Route("api/UsersAPI")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IUserRepository _userRepository;

        public UsersController(ApplicationDbContext db, IUserRepository userRepository)
        {
            _db = db;
            _userRepository = userRepository;
        }

        [EnableCors("BasicPolicy")]
        [HttpGet("Authenticate")]
        [Authorize]
        public async Task<List<string>> Get()
        {
            return await _userRepository.GetUserNames();
        }

        [EnableCors("BasicPolicy")]
        [HttpGet(Name = "GetAllUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<UsersDTO>> GetAllUsers()
        {
            return Ok(_db.Users.ToList());
        }

        [EnableCors("BasicPolicy")]
        [HttpGet("{id:long}", Name = "GetUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<UsersDTO> GetUserById(long id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var user = _db.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [EnableCors("BasicPolicy")]
        [HttpPost("LoginUsersRet")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<UsersDTO> LoginUsersRet([FromBody] LoginDTO loginDTO)
        {
            var user = _db.Users.FirstOrDefault(u => u.UserName == loginDTO.UserName);
            if (user == null)
            {
                return NotFound();
            }

            string password = loginDTO.Password;
            string encodedPassword = user.Password;
            bool isPwdRight = BusinessService.DecodeFrom64(encodedPassword).Equals(password);

            if (!isPwdRight) {

                return BadRequest();
            }

            UsersDTO userDTO = new()
            {
                Id = user.Id,
                Name = user.Name,
                UserName = user.UserName,
                Password = user.Password
            };

            return Ok(userDTO);
        }

        [EnableCors("BasicPolicy")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<UsersDTO> AddUser([FromBody] UsersDTO usersDTO)
        {
            if (_db.Users.FirstOrDefault(u => u.UserName.ToLower() == usersDTO.UserName.ToLower()) != null || usersDTO.UserName.ToLower() == "admin")
            {
                ModelState.AddModelError("CustomError", "UserName already Exists!");
                return BadRequest(ModelState);
            }
            if (usersDTO == null)
            {
                return BadRequest(usersDTO);
            }
            if (usersDTO.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            Users user = new()
            {
                Id = usersDTO.Id,
                Name = usersDTO.Name,
                UserName = usersDTO.UserName,
                Password = BusinessService.EncodePasswordToBase64(usersDTO.Password)
            };

            _db.Users.Add(user);
            _db.SaveChanges();

            return CreatedAtRoute("GetUser", new { id = usersDTO.Id }, usersDTO);
        }

        [EnableCors("BasicPolicy")]
        [HttpDelete("{id:long}", Name = "DeleteUser")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteUser(long id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var user = _db.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            _db.Users.Remove(user);
            _db.SaveChanges();

            return NoContent();
        }

        [EnableCors("BasicPolicy")]
        [HttpPut("{id:long}", Name = "UpdateUser")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<UsersDTO> UpdateUser([FromBody] UsersDTO usersDTO, long id)
        {

            if (usersDTO == null || id != usersDTO.Id)
            {
                return BadRequest();
            }

            Users model = new()
            {
                Id = usersDTO.Id,
                Name = usersDTO.Name,
                UserName = usersDTO.UserName,
                Password = BusinessService.EncodePasswordToBase64(usersDTO.Password)
            };

            _db.Users.Update(model);
            _db.SaveChanges();

            return NoContent();
        }

        [EnableCors("BasicPolicy")]
        [HttpPatch("{id:long}", Name = "UpdatePartialUser")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartialUser(JsonPatchDocument<UsersDTO> patchDTO, long id)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            var user = _db.Users.AsNoTracking().FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return BadRequest();
            }

            UsersDTO userDTO = new()
            {
                Id = user.Id,
                Name = user.Name,
                UserName = user.UserName,
                Password = user.Password
            };

            patchDTO.ApplyTo(userDTO, ModelState);

            Users model = new()
            {
                Id = userDTO.Id,
                Name = userDTO.Name,
                UserName = userDTO.UserName,
                Password = userDTO.Password
            };

            _db.Users.Update(model);
            _db.SaveChanges();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return NoContent();
        }

    }
}
