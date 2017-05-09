using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application_DbAccess
{
    public class RepositoryMyMovie
    {
            private readonly ApplicationContext _context;
            private DbSet<MyMovie> mymovies;
            string errorMessage = string.Empty;

            public RepositoryMyMovie(ApplicationContext context)
            {
                _context = context;
                mymovies = context.Set<MyMovie>();
            }
            public IEnumerable<MyMovie> GetAll()
            {
                return mymovies.AsEnumerable();
            }

        public MyMovie Get(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("MovieSet");
            }
            return mymovies.SingleOrDefault(s => s.MyMovieID == id);
        }
        public void Insert(MyMovie mymovie)
        {
            if (mymovie == null)
            {
                throw new ArgumentNullException("MyMovie");
            }
            mymovies.Add(mymovie);
            _context.SaveChanges();
        }

        public void Update(MyMovie mymovie)
        {
            if (mymovie == null)
            {
                throw new ArgumentNullException("MyMovie");
            }
            MyMovie dbMyMovie = _context.Find<MyMovie>(mymovie.MyMovieID);
            dbMyMovie.MovieID = mymovie.MovieID;
            dbMyMovie.MyMovieComment = mymovie.MyMovieComment;
            dbMyMovie.MyMovieRating = mymovie.MyMovieRating;
            _context.Update(dbMyMovie);
            _context.SaveChanges();
        }

        public void Delete(MyMovie mymovie)
            {
                if (mymovie == null)
                {
                    throw new ArgumentNullException("MyMovie");
                }
                mymovies.Remove(mymovie);
                _context.SaveChanges();
            }
        }
    }


