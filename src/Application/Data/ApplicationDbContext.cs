using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Application.Models;
using Application_DbAccess;

namespace Application.Data
{
    public class ApplicationDbContext : IdentityDbContext<Models.ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Application_DbAccess.ApplicationUser>().ToTable("Users")
                .HasMany(x => x.MovieSets)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserID);

        }

        //public DbSet<Movie> Movies { get; set; }

    }
}
