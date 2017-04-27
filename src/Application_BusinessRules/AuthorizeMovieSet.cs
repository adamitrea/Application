using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application_DbAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Application_BusinessRules
{
    public interface IAuthorizeMovieSet
    {
        bool CheckUserId(int ?id, ICurrentUserId currentUserId, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager, RepositoryMovieSet repoMovieSet);

        bool MovieSetExists(int id, RepositoryMovieSet repoMovieSet);
    }
    public class AuthorizeMovieSet: IAuthorizeMovieSet
    {

        public bool CheckUserId(int? id, ICurrentUserId currentUserId, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager, RepositoryMovieSet repoMovieSet)
        {
            if(id == null)
            {
                throw new ArgumentNullException("Null");
            }

            var userid = currentUserId.GetCurrentUserId(httpContextAccessor, userManager);

            MovieSet movieSet = repoMovieSet.GetSingle(id);

            return userid.Equals(movieSet.UserID);
        }

        public bool MovieSetExists(int id, RepositoryMovieSet repoMovieSet)
        {
            return repoMovieSet.GetAll().Any(e => e.MovieSetID == id);
        }
    }
}
