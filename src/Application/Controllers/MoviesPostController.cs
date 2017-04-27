using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Application.Data;
using Microsoft.AspNetCore.Authorization;
using Application_DbAccess;
using Application_BusinessRules;

namespace Application.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/MoviesPost")]
    public class MoviesPostController : Controller
    {
        private readonly ApplicationContext _context;

        private readonly RepositoryMovie _repoMovie;

        private readonly IAuthorizeMovie _authmovie;

        public MoviesPostController(ApplicationContext context, RepositoryMovie repoMovie, IAuthorizeMovie authmovie)
        {
            _context = context;
            _repoMovie = repoMovie;
            _authmovie = authmovie;
        }

        // GET: api/MoviesPost
        [HttpGet]
        public IEnumerable<Movie> GetMovie()
        {
            return _repoMovie.GetAll();
        }

        
        // POST: api/MoviesPost
        [HttpPost]
        public IActionResult PostMovie([FromBody] Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _repoMovie.Insert(movie);
            }
            catch (DbUpdateException)
            {
                if (_authmovie.MovieExists(movie.MovieID,movie.TMDb,_repoMovie))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetMovie", new { id = movie.MovieID }, movie);
        }

    }
}