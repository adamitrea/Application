using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application_DbAccess;

namespace Application_BusinessRules
{
    public interface IAuthorizeMyMovie
    {
        bool CheckMovie(RepositoryMovieSet repomovieset, MyMovie myMovie);

        bool MyMovieExists(int id, RepositoryMyMovie repomymovie);
    }

    public class AuthorizeMyMovie: IAuthorizeMyMovie
    {
        public bool CheckMovie(RepositoryMovieSet repomovieset, MyMovie myMovie)
        {
            MovieSet movieset = repomovieset.Get(myMovie.MovieSetID);

            return movieset.MyMovies.Any(e => e.MovieID == myMovie.MovieID);
        }

        public bool MyMovieExists(int id, RepositoryMyMovie repomymovie)
        {
            return repomymovie.GetAll().Any(e => e.MyMovieID == id);
        }
    }
}
