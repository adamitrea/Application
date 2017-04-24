using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Application.Data;
using Application.Models;
using Microsoft.AspNetCore.Authorization;

namespace Application.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/MoviesPost")]
    public class MoviesPostController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MoviesPostController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/MoviesPost
        [HttpGet]
        public IEnumerable<Movie> GetMovie()
        {
            return _context.Movies;
        }

        
        // POST: api/MoviesPost
        [HttpPost]
        public async Task<IActionResult> PostMovie([FromBody] Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Movies.Add(movie);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MovieExists(movie.MovieID,movie.TMDb))
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


        private bool MovieExists(int id, int tmdb)
        {
            if (_context.Movies.Any(e => e.MovieID == id)) return true;
            else return _context.Movies.Any(e => e.TMDb == tmdb);

        }
    }
}