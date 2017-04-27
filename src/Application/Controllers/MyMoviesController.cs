using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Application.Data;
using Application_DbAccess;
using Microsoft.AspNetCore.Authorization;
using Application_BusinessRules;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace Application.Controllers
{
    [Authorize]
    public class MyMoviesController : Controller
    {
        private readonly ApplicationContext _context;

        private readonly RepositoryMyMovie _repoMyMovie;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ICurrentUserId _currentuserid;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly RepositoryMovieSet _movieset;

        private readonly IAuthorizeMovieSet _authmovieset;

        private readonly IAuthorizeMyMovie _authmymovie;

        public MyMoviesController(ApplicationContext context, RepositoryMyMovie repoMyMovie, ICurrentUserId currentuserid, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager, RepositoryMovieSet movieset, IAuthorizeMovieSet authmovieset, IAuthorizeMyMovie authmymovie)
        {
            _context = context;
            _repoMyMovie = repoMyMovie;
            _currentuserid = currentuserid;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _movieset = movieset;
            _authmovieset = authmovieset;
            _authmymovie = authmymovie;
        }



        // GET: MyMovies/Create
        public IActionResult Create(int id)
        {
            if (!_authmovieset.CheckUserId(id, _currentuserid, _httpContextAccessor, _userManager, _movieset))
            {
                return Unauthorized();
            }
            var myMovie = new MyMovie();
            myMovie.MovieSetID = id;
            return View(myMovie);
        }

        // POST: MyMovies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MyMovieID,CreationDate,MovieID,MovieSetID,MyMovieComment,MyMovieRating")] MyMovie myMovie)
        {
            if (ModelState.IsValid)
            {
                if (!_authmovieset.CheckUserId(myMovie.MovieSetID, _currentuserid, _httpContextAccessor, _userManager, _movieset))
                {
                    return Unauthorized();
                }
                if (await _authmymovie.CheckMovie(_movieset, myMovie))
                {
                    return NoContent();
                }
                myMovie.CreationDate = DateTime.Today;
                _repoMyMovie.Insert(myMovie);
                return RedirectToAction("Details", "MovieSets", new { id = myMovie.MovieSetID });
            }
            ViewData["MovieID"] = new SelectList(_context.Movies, "MovieID", "MovieName", myMovie.MovieID);
            ViewData["MovieSetID"] = new SelectList(_context.MovieSets, "MovieSetID", "MovieSetID", myMovie.MovieSetID);
            return View(myMovie);
        }

        // GET: MyMovies/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var myMovie = _repoMyMovie.Get(id);
            if (!_authmovieset.CheckUserId(myMovie.MovieSetID, _currentuserid, _httpContextAccessor, _userManager, _movieset))
            {
                return Unauthorized();
            }

            //ViewData["MovieID"] = new SelectList(_context.Movies, "MovieID", "MovieName", myMovie.MovieID);
            // ViewData["MovieSetID"] = new SelectList(_context.MovieSets, "MovieSetID", "MovieSetID", myMovie.MovieSetID);
            return View(myMovie);
        }

        // POST: MyMovies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("MyMovieID,CreationDate,MovieID,MovieSetID,MyMovieComment,MyMovieRating")] MyMovie myMovie)
        {
            if (id != myMovie.MyMovieID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (!_authmovieset.CheckUserId(myMovie.MovieSetID, _currentuserid, _httpContextAccessor, _userManager, _movieset))
                {
                    return Unauthorized();
                }
                try
                {
                    _repoMyMovie.Update(myMovie);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_authmymovie.MyMovieExists(myMovie.MyMovieID, _repoMyMovie))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "MovieSets", new { id = myMovie.MovieSetID });
            }
            //ViewData["MovieID"] = new SelectList(_context.Movies, "MovieID", "MovieID", myMovie.MovieID);
            //ViewData["MovieSetID"] = new SelectList(_context.MovieSets, "MovieSetID", "MovieSetID", myMovie.MovieSetID);
            return View(myMovie);
        }

        // GET: MyMovies/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var myMovie = _repoMyMovie.Get(id);
            if (!_authmovieset.CheckUserId(myMovie.MovieSetID, _currentuserid, _httpContextAccessor, _userManager, _movieset))
            {
                return Unauthorized();
            }
            if (myMovie == null)
            {
                return NotFound();
            }

            return View(myMovie);
        }

        // POST: MyMovies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var myMovie = _repoMyMovie.Get(id);
            if (!_authmovieset.CheckUserId(myMovie.MovieSetID, _currentuserid, _httpContextAccessor, _userManager, _movieset))
            {
                return Unauthorized();
            }
            var setid = myMovie.MovieSetID;
            _repoMyMovie.Delete(myMovie);
            return RedirectToAction("Details", "MovieSets", new { id = setid });
        }

    }
}
