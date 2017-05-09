using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Application_DbAccess;
using Microsoft.AspNetCore.Http;
using Application.Services;

namespace Application.ViewComponents
{
    public class MovieSetsViewComponent: ViewComponent
    {
        private RepositoryMovieSet _repoMovieSet;


        public MovieSetsViewComponent(RepositoryMovieSet repoMovieSet)
        {
            _repoMovieSet = repoMovieSet;
        }

        //private ApplicationContext _context;

        //public MovieSetsViewComponent(ApplicationContext context)
        //{
        //    _context = context;
        //}

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            var items = await GetItemsAsync(id);
            return View(items);
        }

        private async Task<List<MovieSet>> GetItemsAsync(string id)
        {
            var movieSets = new List<MovieSet>();

            var sets = _repoMovieSet.GetAll();

            foreach(MovieSet s in sets)
            {
                if (s.UserID == id) movieSets.Add(s);
            }
            return movieSets;
        }
    }
}
