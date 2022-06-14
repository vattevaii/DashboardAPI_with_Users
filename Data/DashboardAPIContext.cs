using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DashboardAPI.Models;

namespace DashboardAPI.Data
{
    public class DashboardAPIContext : DbContext
    {
        public DashboardAPIContext (DbContextOptions<DashboardAPIContext> options)
            : base(options)
        {
        }

        public DbSet<DashboardAPI.Models.User> User { get; set; } = null!;
    }
}
