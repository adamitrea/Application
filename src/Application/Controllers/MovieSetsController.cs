using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Application.Data;
using Application_DbAccess;
using Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Application.Models;
using Application_BusinessRules;

namespace Application.Controllers
{
    [Authorize]
    public class MovieSetsController : Controller
    {
        private readonly ApplicationContext _context;

        private readonly ApplicationDbContext _contextDb;

        private readonly ICurrentUserId _currentuserid;

        private readonly UserManager<Application_DbAccess.ApplicationUser> _userManager;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly RepositoryMovieSet _movieset;

        private readonly IAuthorizeMovieSet _authmovieset;

        public MovieSetsController(ApplicationContext context, ICurrentUserId currentuserid, IHttpContextAccessor httpContextAccessor, UserManager<Application_DbAccess.ApplicationUser> userManager, ApplicationDbContext contextDb, RepositoryMovieSet movieset, IAuthorizeMovieSet authmovieset)
        {
            _context = context;
            _contextDb = contextDb;
            _currentuserid = currentuserid;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _movieset = movieset;
            _authmovieset = authmovieset;
        }


        // GET: MovieLists
        public IActionResult Index()
        {
            var applicationContext = _movieset.GetAll()
                .Where(x => x.UserID == _currentuserid.GetCurrentUserId(_httpContextAccessor,_userManager));

            return View(applicationContext.ToList());
        }

        // GET: MovieLists/Details/5
        public IActionResult Details(int? id)
        {
            if(!_authmovieset.CheckUserId(id,_currentuserid,_httpContextAccessor,_userManager,_movieset))
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
                movieSet.UserID = _currentuserid.GetCurrentUserId(_httpContextAccessor, _userManager);
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
            if (!_authmovieset.CheckUserId(id, _currentuserid, _httpContextAccessor, _userManager, _movieset))
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
                    if (!_authmovieset.MovieSetExists(movieSet.MovieSetID, _movieset))
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
            if (!_authmovieset.CheckUserId(id, _currentuserid, _httpContextAccessor, _userManager, _movieset))
            {
                return Unauthorized();
            }
            var movieSet = _movieset.GetSingle(id);
            _movieset.Delete(movieSet);
            return RedirectToAction("Index");
        }


    }
}

