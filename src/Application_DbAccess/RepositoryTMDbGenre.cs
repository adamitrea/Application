using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application_DbAccess
{
    public class RepositoryTMDbGenre
    {
        private readonly ApplicationContext _context;
        private DbSet<TMDbGenre> genres;
        string errorMessage = string.Empty;

        public RepositoryTMDbGenre(ApplicationContext context)
        {
            _context = context;
            genres = context.Set<TMDbGenre>();
        }
        public IEnumerable<TMDbGenre> GetAll()
        {
            return genres.AsEnumerable();
        }

        public TMDbGenre Get(long id)
        {
            return genres.SingleOrDefault(s => s.TMDbGenreID == id);
        }
        public void Insert(TMDbGenre genre)
        {
            if (genre == null)
            {
                throw new ArgumentNullException("Movie");
            }
            genres.Add(genre);
            _context.SaveChanges();
        }

        public void Update(TMDbGenre genre)
        {
            if (genre == null)
            {
                throw new ArgumentNullException("Movie");
            }
            _context.Update(genre);
            _context.SaveChanges();
        }

        public void Delete(TMDbGenre genre)
        {
            if (genre == null)
            {
                throw new ArgumentNullException("Movie");
            }
            genres.Remove(genre);
            _context.SaveChanges();
        }
    }
}
