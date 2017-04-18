//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Application.Models;
//using Microsoft.EntityFrameworkCore;

//namespace Application.Data
//{
//    public class ApplicationContext: DbContext

//    {
//        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
//        {
//        }

//        public DbSet<User> Users { get; set; }
//        public DbSet<MovieSet> MovieSets { get; set; }
//        public DbSet<Movie> Movies { get; set; }
//        public DbSet<MyMovie> MyMovies { get; set; }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            modelBuilder.Entity<User>().ToTable("User");
//            modelBuilder.Entity<MovieSet>().ToTable("MovieSet");
//            modelBuilder.Entity<Movie>().ToTable("Movie");
//            modelBuilder.Entity<MyMovie>().ToTable("MyMovie");
//        }
//    }
//}
