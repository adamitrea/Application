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
        public MovieSet Get(int ?id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("MovieSet");
            }
            return moviesets.Include(x => x.MyMovies)
                                .ThenInclude(x => x.Movie)
                                    .SingleOrDefault(s => s.MovieSetID == id);
        }
        public void Insert(MovieSet movieset)
        {
            if (movieset == null)
            {
                throw new ArgumentNullException("MovieSet");
            }
            moviesets.Add(movieset);
            _context.SaveChanges();
        }

        public void Update(MovieSet movieset)
        {
            if (movieset == null)
            {
                throw new ArgumentNullException("MovieSet");
            }
            var dbMovieSet = _context.Find<MovieSet>(movieset.MovieSetID);
            dbMovieSet.SetName = movieset.SetName;
            _context.Update(dbMovieSet);
            _context.SaveChanges();

        }

        public void Delete(MovieSet movieset)
        {
            if (movieset == null)
            {
                throw new ArgumentNullException("MovieSet");
            }
            moviesets.Remove(movieset);
            _context.SaveChanges();
        }
    }
}
