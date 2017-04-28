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
        bool CheckUserId(int ?id, IHttpContextAccessor httpContextAccessor);

        bool MovieSetExists(int id);
    }
    public class AuthorizeMovieSet: IAuthorizeMovieSet
    {
        private RepositoryMovieSet repoMovieSet;
        private ICurrentUserService currentUserService;

        public AuthorizeMovieSet(ICurrentUserService currentUserService, RepositoryMovieSet repoMovieSet)
        {
            this.currentUserService = currentUserService;
            this.repoMovieSet = repoMovieSet;
        }

        public bool CheckUserId(int? id, IHttpContextAccessor httpContextAccessor)
        {
            if(id == null)
            {
                throw new ArgumentNullException("Null");
            }

            var userid = currentUserService.GetCurrentUserId(httpContextAccessor);

            MovieSet movieSet = repoMovieSet.GetSingle(id);

            return userid.Equals(movieSet.UserID);
        }

        public bool MovieSetExists(int id)
        {
            return repoMovieSet.GetAll().Any(e => e.MovieSetID == id);
        }
    }
}
