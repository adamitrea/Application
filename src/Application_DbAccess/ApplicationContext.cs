using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Application_DbAccess
{
    public class ApplicationContext : IdentityDbContext
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
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(x => x.MovieSets)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserID);
            modelBuilder.Entity<Movie>().ToTable("Movie");
            modelBuilder.Entity<MyMovie>().ToTable("MyMovie");
            modelBuilder.Entity<MovieSet>().ToTable("MovieSets");
            modelBuilder.Entity<TMDbGenre>().ToTable("TMDbGenre");
        }

        //public DbSet<Movie> Movies { get; set; }
       
    }
}
