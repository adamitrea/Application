using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application_DbAccess;

namespace Application_BusinessRules
{
    public interface IAuthorizeMovie
    {
        bool MovieExists(int id, int tmdb, RepositoryMovie _repoMovie);
    }
    public class AuthorizeMovie: IAuthorizeMovie
    {
        public bool MovieExists(int id, int tmdb, RepositoryMovie _repoMovie)
        {
            if (_repoMovie.GetAll().Any(e => e.MovieID == id)) return true;
            else return _repoMovie.GetAll().Any(e => e.TMDb == tmdb);

        }
    }
}
