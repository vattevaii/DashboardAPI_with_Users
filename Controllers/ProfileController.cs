
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DashboardAPI.Data;
using DashboardAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace DashboardAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly DashboardAPIContext _context;
        private readonly IWebHostEnvironment _env;

        public ProfileController(DashboardAPIContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: api/Profile
        [HttpGet("image")]
        public PhysicalFileResult GetImg()
        {
            var filePath = Path.Combine(_env.ContentRootPath, "Static","Icon.png");
            return PhysicalFile(filePath, "images/png");
        }
    }
}
