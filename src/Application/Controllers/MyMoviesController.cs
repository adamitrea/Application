using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Application.Models;
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

        private readonly RepositoryMyMovie _repoMyMovie;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly RepositoryMovieSet _movieset;

        private readonly IAuthorizeMovieSet _authmovieset;

        private readonly IAuthorizeMyMovie _authmymovie;

        public MyMoviesController( RepositoryMyMovie repoMyMovie, ICurrentUserService currentuserid, IHttpContextAccessor httpContextAccessor, RepositoryMovieSet movieset, IAuthorizeMovieSet authmovieset, IAuthorizeMyMovie authmymovie)
        {
            _repoMyMovie = repoMyMovie;
            _httpContextAccessor = httpContextAccessor;
            _movieset = movieset;
            _authmovieset = authmovieset;
            _authmymovie = authmymovie;
        }



        // GET: MyMovies/Create
        public IActionResult Create(int id)
        {
            if (!_authmovieset.CheckUserId(id, _httpContextAccessor))
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
        public IActionResult Create([Bind("MyMovieID,CreationDate,MovieID,MovieSetID,MyMovieComment,MyMovieRating")] MyMovie myMovie)
        {
            if (ModelState.IsValid)
            {
                if (!_authmovieset.CheckUserId(myMovie.MovieSetID, _httpContextAccessor))
                {
                    return Unauthorized();
                }
                if (_authmymovie.CheckMovie(_movieset, myMovie))
                {
                    return NoContent();
                }
                myMovie.CreationDate = DateTime.Today;
                _repoMyMovie.Insert(myMovie);
                return RedirectToAction("Details", "MovieSets", new { id = myMovie.MovieSetID });
            }
            //TODO: daca nu merge lista de my moveis -> HERE
            //ViewData["MovieID"] = new SelectList(_context.Movies, "MovieID", "MovieName", myMovie.MovieID);
            //ViewData["MovieSetID"] = new SelectList(_context.MovieSets, "MovieSetID", "MovieSetID", myMovie.MovieSetID);
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
            if (!_authmovieset.CheckUserId(myMovie.MovieSetID, _httpContextAccessor))
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
                if (!_authmovieset.CheckUserId(myMovie.MovieSetID, _httpContextAccessor))
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
            if (!_authmovieset.CheckUserId(myMovie.MovieSetID, _httpContextAccessor))
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
            if (!_authmovieset.CheckUserId(myMovie.MovieSetID, _httpContextAccessor))
            {
                return Unauthorized();
            }
            var setid = myMovie.MovieSetID;
            _repoMyMovie.Delete(myMovie);
            return RedirectToAction("Details", "MovieSets", new { id = setid });
        }

    }
}
