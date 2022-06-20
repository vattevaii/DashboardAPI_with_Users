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
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // user hasmany products and orders
            builder.Entity<User>()
                .HasMany(u => u.Products)
                .WithOne(p => p.belongsTo);
            builder.Entity<User>()
                .HasMany(u => u.Orders)
                .WithOne(p => p.OrderedBy);
        }
        public DbSet<DashboardAPI.Models.User> User { get; set; } = null!;
        public DbSet<DashboardAPI.Models.Product> Product { get; set; } = null!;
        public DbSet<DashboardAPI.Models.Orders> Order { get; set; } = null!;
    }
}
