using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Application.Models;

namespace Application.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

       // public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<MovieSet> MovieSets { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MyMovie> MyMovies { get; set; }
        public DbSet<TMDbGenre> TMDbGenres { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            //modelBuilder.Entity<ApplicationUser>().ToTable("User");
            modelBuilder.Entity<MovieSet>().ToTable("MovieSet")
                .HasOne(x => x.User)
                .WithMany(x => x.MovieSets)
                .HasForeignKey(x => x.UserID);
                //.HasConstraintName(UserID);
            modelBuilder.Entity<Movie>().ToTable("Movie");
            modelBuilder.Entity<MyMovie>().ToTable("MyMovie");
            modelBuilder.Entity<TMDbGenre>().ToTable("TMDbGenre");
        }

        //public DbSet<Movie> Movies { get; set; }

    }
}
