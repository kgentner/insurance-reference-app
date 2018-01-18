using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Reggie.Models;

namespace Reggie.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Reggie.Models.Insurance> Insurances { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Reggie.Models.Insurance>()
            .HasIndex(k => new { k.Payor, k.Plan, k.Location_AK, k.Location_CA, k.Location_MT, k.Location_OR, k.Location_WA })
            .IsUnique();
        }
    }
}
