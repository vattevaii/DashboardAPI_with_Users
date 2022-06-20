using Microsoft.AspNetCore.Mvc;
using DashboardAPI.Models;

using DashboardAPI.Data;
using DashboardAPI.AuthFunctions;
using Microsoft.AspNetCore.Authorization;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DashboardAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DashboardAPIContext _context;
        private IConfiguration _config;
        private Authority _auth;

        public AuthController(IConfiguration configuration, DashboardAPIContext context)
        {
            _config = configuration;
            _context = context;
            _auth = new Authority(_config);
        }

        // POST api/<ValuesController>
        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register([FromBody] UserDTO rBody)
        {
            _auth.CreatePasswordHash(rBody.Password, out byte[] passwordHash, out byte[] passwordSalt);
            User user = new User();
            user.Username = rBody.Username;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            _context.User.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtRoute("GetUser", new { id = user.Id }, user.toDTO());
        }
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] UserDTO rBody)
        {
            User? user = _context.User.FirstOrDefault(u => u.Username == rBody.Username);
            if (user == null)
                return BadRequest("Invalid username");
            var verify = _auth.VerifyPasswordHash(rBody.Password, user.PasswordHash, user.PasswordSalt);
            if (!verify)
                return BadRequest("Invalid password");
            string token = _auth.CreateToken(user);
            return Ok(token);
        }

        [HttpPost("password")]
        [Authorize]
        public async Task<ActionResult<UserAction>> EditPassword([FromBody] string password)
        {
            if (_context.User == null)
            {
                return NotFound();
            }
            int id = int.Parse(s: User.FindFirst("User").Value);
            var user = await _context.User.FindAsync(id);
            _auth.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            await _context.SaveChangesAsync();
            
            return new UserAction(user,"Password changed successfully");
        }

        // PUT api/<ValuesController>/5

    }
}
