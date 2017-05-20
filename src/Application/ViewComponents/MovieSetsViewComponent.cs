using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application_DbAccess;

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
            var items = GetItems(id);
            return View(items);
        }

        private List<MovieSet> GetItems(string id)
        {

            var sets = _repoMovieSet.GetAll()
                .Where(x => x.UserID == id);

            return sets.ToList();
        }
    }
}
