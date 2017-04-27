using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Application_DbAccess
{
    public class ApplicationContext :DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }
        
        public DbSet<MovieSet> MovieSets { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MyMovie> MyMovies { get; set; }
        public DbSet<TMDbGenre> TMDbGenres { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<MovieSet>().ToTable("MovieSet");
                ////.HasOne(x => x.User)
                ////.WithMany(x => x.MovieSets)
                //.HasForeignKey(x => x.UserID);
            modelBuilder.Entity<Movie>().ToTable("Movie");
            modelBuilder.Entity<MyMovie>().ToTable("MyMovie");
            modelBuilder.Entity<TMDbGenre>().ToTable("TMDbGenre");
            modelBuilder.Entity<ApplicationUser>().ToTable("Users");
        }

        //public DbSet<Movie> Movies { get; set; }

    }
}
