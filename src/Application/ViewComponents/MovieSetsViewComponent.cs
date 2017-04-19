using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Application.Models;
using Application.Data;

namespace Application.ViewComponents
{
    public class MovieSetsViewComponent: ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public MovieSetsViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await GetItemsAsync();
            return View(items);
        }

        private Task<List<MovieSet>> GetItemsAsync()
        {
            return _context.MovieSets.ToListAsync();
        }
    }
}
