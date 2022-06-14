using Microsoft.AspNetCore.Mvc;
using DashboardAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

using DashboardAPI.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DashboardAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new User();
        private readonly DashboardAPIContext _context;
        private IConfiguration _config;

        public AuthController(IConfiguration configuration, DashboardAPIContext context)
        {
            _config = configuration;
            _context = context;
        }

        // POST api/<ValuesController>
        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register([FromBody] UserDTO rBody)
        {
            CreatePasswordHash(rBody.Password, out byte[] passwordHash, out byte[] passwordSalt);
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
            if (!VerifyPasswordHash(rBody.Password, user.PasswordHash, user.PasswordSalt))
                return BadRequest("Invalid password");
            string token = CreateToken(user);
            return Ok(token);
        }

        // PUT api/<ValuesController>/5
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(storedHash);
            }
            //return true;
        }

        private string CreateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                //issuer: "http://localhost:5000",
                //audience: "http://localhost:5000",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
