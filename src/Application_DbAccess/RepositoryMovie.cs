using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application_DbAccess
{
    public class RepositoryMovie
    {
        private readonly ApplicationContext _context;
        private DbSet<Movie> movies;
        string errorMessage = string.Empty;

        public RepositoryMovie(ApplicationContext context)
        {
            _context = context;
            movies = context.Set<Movie>();
        }
        public IEnumerable<Movie> GetAll()
        {
            return movies.AsEnumerable();
        }

        public async Task<Movie> Get(long id)
        {
            return await movies.SingleOrDefaultAsync(s => s.MovieID == id);
        }
        public void Insert(Movie movie)
        {
            if (movie == null)
            {
                throw new ArgumentNullException("Movie");
            };
            movies.Add(movie);
            _context.SaveChanges();
        }

        public void Update(Movie movie)
        {
            if (movie == null)
            {
                throw new ArgumentNullException("Movie");
            }
            _context.Update(movie);
            _context.SaveChanges();
        }

        public void Delete(Movie movie)
        {
            if (movie == null)
            {
                throw new ArgumentNullException("Movie");
            }
            movies.Remove(movie);
            _context.SaveChanges();
        }
    }
}
