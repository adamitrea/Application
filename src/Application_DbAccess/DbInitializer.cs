using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Application_DbAccess;
//using Application.Models;

namespace Application.Models
{
    public static class DbInitializer
    {
        public static async void Initialize(ApplicationContext applicationContext, UserManager<ApplicationUser> userManager)
        {
             applicationContext.Database.EnsureCreated();

            if (applicationContext.Movies.Any())
            {
                return;
            }

            ApplicationUser user = new ApplicationUser
            {
                UserName = "herecomesjohnny@murder.com",
                Email = "herecomesjohnny@murder.com",
                EmailConfirmed = true
            };
            IdentityResult result = await userManager.CreateAsync(user, "HereComesJohnny!123");
            if (!result.Succeeded)
            {
                throw new Exception("Couldn't initialize user");
            }


            //applicationContext.Users.Add(user);
            //applicationContext.SaveChanges();
            var movies = new Movie[]
        {
            new Movie{MovieName="Donnie Darko",TMDb=1,MovieYear="2001",MovieDescription="",MovieIcon="",MovieRating=8.1,MovieGenre="Drama, Sci-Fi, Thriller",},
            new Movie{MovieName="Inception",TMDb=2,MovieYear="2010",MovieDescription="",MovieIcon="",MovieRating=8.8,MovieGenre="Action, Adventure, Sci-Fi",},
            new Movie{MovieName="Pan's Labyrinth",TMDb=3,MovieYear="2006",MovieDescription="",MovieIcon="",MovieRating=8.2,MovieGenre="Drama, Fantasy, War",},
            new Movie{MovieName="A Beautiful Mind",TMDb=4,MovieYear="2001",MovieDescription="",MovieIcon="",MovieRating=8.2,MovieGenre="Biography, Drama",},
        };
            foreach (Movie m in movies)
            {
                applicationContext.Movies.Add(m);
            }
            applicationContext.SaveChanges();

            var moviesets = new MovieSet[]
{
            new MovieSet{UserID=user.Id,SetName="SF",CreationDate=DateTime.Parse("2005-09-01"),},
            new MovieSet{UserID=user.Id,SetName="Drama",CreationDate=DateTime.Parse("2015-10-07"),},
            new MovieSet{UserID=user.Id,SetName="Filme3",CreationDate=DateTime.Parse("2014-11-13"),},
            new MovieSet{UserID=user.Id,SetName="Filme4",CreationDate=DateTime.Parse("2013-09-15"),},

};
            foreach (MovieSet l in moviesets)
            {
                applicationContext.MovieSets.Add(l);
            }
            applicationContext.SaveChanges();

            var mymovies = new MyMovie[]
{
            new MyMovie{MyMovieRating=9.5,
                MovieID = movies.Single(s => s.MovieName == "Donnie Darko").MovieID,
                MovieSetID = moviesets.Single(s => s.SetName == "SF").MovieSetID,},
            new MyMovie{MyMovieRating=9,
                MovieID = movies.Single(s => s.MovieName == "Inception").MovieID,
                MovieSetID = moviesets.Single(s => s.SetName == "SF").MovieSetID,},
            new MyMovie{MyMovieRating=9.3,
                MovieID = movies.Single(s => s.MovieName == "Pan's Labyrinth").MovieID,
                MovieSetID = moviesets.Single(s => s.SetName == "Drama").MovieSetID,},
            new MyMovie{MyMovieRating=9,
                MovieID = movies.Single(s => s.MovieName == "A Beautiful Mind").MovieID,
                MovieSetID = moviesets.Single(s => s.SetName == "Drama").MovieSetID,},
};
            foreach (MyMovie my in mymovies)
            {
                applicationContext.MyMovies.Add(my);
            }

            var genres = new TMDbGenre[]
            {
            new TMDbGenre {TMDbID =28,Name="Action" },
            new TMDbGenre {TMDbID =12,Name="Adventure" },
            new TMDbGenre {TMDbID =16,Name="Animation" },
            new TMDbGenre {TMDbID =35,Name="Comedy" },
            new TMDbGenre {TMDbID =80,Name="Crime" },
            new TMDbGenre {TMDbID =99,Name="Documemntary" },
            new TMDbGenre {TMDbID =18,Name="Drama" },
            new TMDbGenre {TMDbID =10751,Name="Family" },
            new TMDbGenre {TMDbID =14,Name="Fantasy" },
            new TMDbGenre {TMDbID =36,Name="History" },
            new TMDbGenre {TMDbID =27,Name="Horror" },
            new TMDbGenre {TMDbID =10402,Name="Music" },
            new TMDbGenre {TMDbID =9648,Name="Mystery" },
            new TMDbGenre {TMDbID =10749,Name="Romance" },
            new TMDbGenre {TMDbID =878,Name="Science Fiction" },
            new TMDbGenre {TMDbID =10770,Name="TV Movie" },
            new TMDbGenre {TMDbID =53,Name="Thriller" },
            new TMDbGenre {TMDbID =10752,Name="War" },
            new TMDbGenre {TMDbID =37,Name="Western" },
            };

            foreach (TMDbGenre g in genres)
            {
                applicationContext.TMDbGenres.Add(g);
            }

            applicationContext.SaveChanges();

        }
    }
}
