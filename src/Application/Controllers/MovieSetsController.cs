using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Application.Data;
using Application.Models;
using Application.Services;

namespace Application.Controllers
{
    public class MovieSetsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly ICurrentUserId _currentuserid;

        public MovieSetsController(ApplicationDbContext context, ICurrentUserId currentuserid)
        {
            _context = context;
            _currentuserid = currentuserid;
        }

        // GET: MovieLists
        public async Task<IActionResult> Index()
        {
            var applicationContext = _context.MovieSets.Include(m => m.User);
            return View(await applicationContext.ToListAsync());
        }

        // GET: MovieLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieSet = await _context.MovieSets
                .Include(x => x.MyMovies)
                    .ThenInclude(x => x.Movie)
                .SingleOrDefaultAsync(m => m.MovieSetID == id);
            if (movieSet == null)
            {
                return NotFound();
            }

            return View(movieSet);
        }

        // GET: MovieLists/Create
        public IActionResult Create()
        {
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "UserID");
            return View();
        }

        // POST: MovieLists/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MovieSetID,SetName,UserID")] MovieSet movieSet)
        {
            if (ModelState.IsValid)
            {
                movieSet.CreationDate = DateTime.Today;
                _context.Add(movieSet);
                movieSet.UserID = _currentuserid.getID();
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "UserID", movieSet.UserID);
            return View(movieSet);
        }

        // GET: MovieLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieSet = await _context.MovieSets.SingleOrDefaultAsync(m => m.MovieSetID == id);
            if (movieSet == null)
            {
                return NotFound();
            }
            return View(movieSet);
        }

        // POST: MovieLists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MovieSetID,SetName,UserID")] MovieSet movieSet)
        {
            if (id != movieSet.MovieSetID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movieSet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieSetExists(movieSet.MovieSetID))
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
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "UserID", movieSet.UserID);
            return View(movieSet);
        }

        // GET: MovieLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieSet = await _context.MovieSets.SingleOrDefaultAsync(m => m.MovieSetID == id);
            if (movieSet == null)
            {
                return NotFound();
            }

            return View(movieSet);
        }

        // POST: MovieLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movieSet = await _context.MovieSets.SingleOrDefaultAsync(m => m.MovieSetID == id);
            _context.MovieSets.Remove(movieSet);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool MovieSetExists(int id)
        {
            return _context.MovieSets.Any(e => e.MovieSetID == id);
        }
    }
}

