using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Application_DbAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Application_BusinessRules;

namespace Application.Controllers
{
    [Authorize]
    public class MovieSetsController : Controller
    {
        private readonly ICurrentUserService _currentuserid;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly RepositoryMovieSet _movieset;

        private readonly IAuthorizeMovieSet _authmovieset;

        public MovieSetsController(ICurrentUserService currentuserid, IHttpContextAccessor httpContextAccessor, RepositoryMovieSet movieset, IAuthorizeMovieSet authmovieset)
        {
            _currentuserid = currentuserid;
            _httpContextAccessor = httpContextAccessor;
            _movieset = movieset;
            _authmovieset = authmovieset;
        }


        // GET: MovieLists
        public IActionResult Index()
        {
            var applicationContext = _movieset.GetAll()
                .Where(x => x.UserID == _currentuserid.GetCurrentUserId(_httpContextAccessor));

            return View(applicationContext.ToList());
        }

        // GET: MovieLists/Details/5
        public IActionResult Details(int? id)
        {
            if (!_authmovieset.CheckUserId(id, _httpContextAccessor))
            {
                return Unauthorized();
            }
            var movieSet = _movieset.Get(id);

            return View(movieSet);
        }

        // GET: MovieLists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MovieLists/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("MovieSetID,SetName,UserID")] MovieSet movieSet)
        {
            if (ModelState.IsValid)
            {
                movieSet.CreationDate = DateTime.Today;
                movieSet.UserID = _currentuserid.GetCurrentUserId(_httpContextAccessor);
                _movieset.Insert(movieSet);
                return RedirectToAction("Index");
            }
            return View(movieSet);
        }

        // GET: MovieLists/Edit/5
        public IActionResult Edit(int? id)
        {

            var movieSet = _movieset.GetSingle(id);

            return View(movieSet);
        }

        // POST: MovieLists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("MovieSetID,SetName,UserID")] MovieSet movieSet)
        {
            if (id != movieSet.MovieSetID)
            {
                return NotFound();
            }
            if (!_authmovieset.CheckUserId(id, _httpContextAccessor))
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _movieset.Update(movieSet);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_authmovieset.MovieSetExists(movieSet.MovieSetID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(movieSet);
        }

        // GET: MovieLists/Delete/5
        public IActionResult Delete(int? id)
        {

            var movieSet = _movieset.GetSingle(id);

            return View(movieSet);
        }

        // POST: MovieLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!_authmovieset.CheckUserId(id, _httpContextAccessor))
            {
                return Unauthorized();
            }
            var movieSet = _movieset.GetSingle(id);
            _movieset.Delete(movieSet);
            return RedirectToAction("Index");
        }


    }
}

