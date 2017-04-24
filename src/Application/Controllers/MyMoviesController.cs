using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Application.Data;
using Application.Models;
using Microsoft.AspNetCore.Authorization;

namespace Application.Controllers
{
    [Authorize]
    public class MyMoviesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MyMoviesController(ApplicationDbContext context)
        {
            _context = context;
        }



        // GET: MyMovies/Create
        public IActionResult Create(int id)
        {
            ViewData["MovieID"] = new SelectList(_context.Movies, "MovieID", "MovieName");
            ViewData["MovieSetID"] = new SelectList(_context.MovieSets, "MovieSetID", "MovieSetID");
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
                myMovie.CreationDate = DateTime.Today;
                _context.Add(myMovie);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "MovieSets", new { id = myMovie.MovieSetID });
            }
            ViewData["MovieID"] = new SelectList(_context.Movies, "MovieID", "MovieName", myMovie.MovieID);
            ViewData["MovieSetID"] = new SelectList(_context.MovieSets, "MovieSetID", "MovieSetID", myMovie.MovieSetID);
            return View(myMovie);
        }

        // GET: MyMovies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var myMovie = await _context.MyMovies.SingleOrDefaultAsync(m => m.MyMovieID == id);
            if (myMovie == null)
            {
                return NotFound();
            }
            ViewData["MovieID"] = new SelectList(_context.Movies, "MovieID", "MovieName", myMovie.MovieID);
            // ViewData["MovieSetID"] = new SelectList(_context.MovieSets, "MovieSetID", "MovieSetID", myMovie.MovieSetID);
            return View(myMovie);
        }

        // POST: MyMovies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MyMovieID,CreationDate,MovieID,MovieSetID,MyMovieComment,MyMovieRating")] MyMovie myMovie)
        {
            if (id != myMovie.MyMovieID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(myMovie);
                    //_context.Update(myMovie.MovieSet.MyMovies);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MyMovieExists(myMovie.MyMovieID))
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
            ViewData["MovieID"] = new SelectList(_context.Movies, "MovieID", "MovieID", myMovie.MovieID);
            //ViewData["MovieSetID"] = new SelectList(_context.MovieSets, "MovieSetID", "MovieSetID", myMovie.MovieSetID);
            return View(myMovie);
        }

        // GET: MyMovies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var myMovie = await _context.MyMovies.SingleOrDefaultAsync(m => m.MyMovieID == id);
            if (myMovie == null)
            {
                return NotFound();
            }

            return View(myMovie);
        }

        // POST: MyMovies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var myMovie = await _context.MyMovies.SingleOrDefaultAsync(m => m.MyMovieID == id);
            var setid = myMovie.MovieSetID;
            _context.MyMovies.Remove(myMovie);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "MovieSets", new { id = setid });
        }

        private bool MyMovieExists(int id)
        {
            return _context.MyMovies.Any(e => e.MyMovieID == id);
        }
    }
}
