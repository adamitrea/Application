using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Application_DbAccess;

namespace Application.Controllers
{
    [Authorize]
    [Produces("application/json")]
    public class SearchController : Controller
    {
        private readonly ApplicationContext _context;

        private readonly RepositoryMovie _repoMovie;

        private readonly RepositoryTMDbGenre _repoGenre;

        public SearchController(ApplicationContext context, RepositoryMovie repoMovie, RepositoryTMDbGenre repoGenre)
        {
            _context = context;
            _repoMovie = repoMovie;
            _repoGenre = repoGenre;
        }

        // GET: api/Search
        [Route("api/Search")]
        [HttpGet]
        public IEnumerable<Movie> GetMovie()
        {
            return _repoMovie.GetAll().ToList();
        }
        [Route("Search")]
        public IActionResult Index()
        {
            return View();
        }

        // GET: api/Search/5
        [Route("api/Search/{pattern}")]
        [HttpGet]
        public async Task<IEnumerable<Movie>> GetMovie([FromRoute] string pattern)
        {
            var movies = new List<Movie>();
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.BaseAddress = new Uri("https://api.themoviedb.org/3/");
            HttpResponseMessage response = await client.GetAsync("search/movie?api_key=91143c47142a2ecb8d02000d1d77e032&language=en-US&query=" + pattern + "&page=1&include_adult=false");
            var jsonResponse = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var tmdb = JsonConvert.DeserializeObject<TMDbMovie>(jsonResponse);
                var genretmdb = _repoGenre.GetAll();
                foreach (var item in tmdb.results)
                {
                    var movie = new Movie();
                    movie.MovieID = 0;
                    movie.MovieName = item.title;
                    movie.MovieYear = item.release_date;
                    movie.MovieDescription = item.overview;
                    movie.MovieRating = item.vote_average;
                    movie.MovieIcon = item.poster_path;
                    foreach (var g in item.genre_ids)
                        foreach (var gt in genretmdb)
                        {
                            if (g.Equals(gt.TMDbID))
                                if(movie.MovieGenre == null) movie.MovieGenre = gt.Name;
                                else movie.MovieGenre = movie.MovieGenre + ", " + gt.Name;
                        }
                    movies.Add(movie);
                }
            }
            return movies;

        }

    }
}