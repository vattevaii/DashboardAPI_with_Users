using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DashboardAPI.Data;
using DashboardAPI.Models;
using Microsoft.AspNetCore.Authorization;

using DashboardAPI.AuthFunctions;


namespace DashboardAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(policy: "User")]
    public class UsersController : ControllerBase
    {
        private readonly DashboardAPIContext _context;

        public UsersController(DashboardAPIContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        [Authorize(policy:"Admin")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUser()
        {
          if (_context.User == null)
          {
              return NotFound();
          }
            List<User> users = await _context.User.ToListAsync();
            List<UserDTO> user = new List<UserDTO> { };
            foreach (User u in users)
            {
                user.Add(u.toDTO());
            }
            return user;
        }

        // GET: api/Users/5
        [HttpGet("{id}", Name = "GetUser")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            if (_context.User == null)
            {
                return NotFound();
            }
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user.toDTO();
        }

        [HttpGet("profile")]
        public async Task<ActionResult<UserDTO>> GetProfile()
        {
            if (_context.User == null)
            {
                return NotFound();
            }
            int id = int.Parse(s: User.FindFirst("User").Value);
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user.toDTO();
        }

        //Edit Profile.. should be only visible to user
        
        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
       
        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        [Authorize(policy: "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_context.User == null)
            {
                return NotFound();
            }
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return (_context.User?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
