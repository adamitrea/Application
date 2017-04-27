using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application_DbAccess
{
    public class RepositoryMovieSet
    {
        private readonly ApplicationContext _context;
        private DbSet<MovieSet> moviesets;
        string errorMessage = string.Empty;

        public RepositoryMovieSet(ApplicationContext context)
        {
            _context = context;
            moviesets = context.Set<MovieSet>();
        }
        public IEnumerable<MovieSet> GetAll()
        {
            return moviesets.Include(m => m.User).AsEnumerable();
        }
        public MovieSet GetSingle(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("MovieSet");
            }
            return moviesets.SingleOrDefault(s => s.MovieSetID == id);
        }
        public async Task<MovieSet> Get(int ?id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("MovieSet");
            }
            return await moviesets.Include(x => x.MyMovies)
                                .ThenInclude(x => x.Movie)
                                    .SingleOrDefaultAsync(s => s.MovieSetID == id);
        }
        public async void Insert(MovieSet movieset)
        {
            if (movieset == null)
            {
                throw new ArgumentNullException("MovieSet");
            }
            moviesets.Add(movieset);
            await _context.SaveChangesAsync();
        }

        public void Update(MovieSet movieset)
        {
            if (movieset == null)
            {
                throw new ArgumentNullException("MovieSet");
            }
            _context.Update(movieset);
                _context.SaveChanges();

        }

        public async void Delete(MovieSet movieset)
        {
            if (movieset == null)
            {
                throw new ArgumentNullException("MovieSet");
            }
            moviesets.Remove(movieset);
            await _context.SaveChangesAsync();
        }
    }
}
