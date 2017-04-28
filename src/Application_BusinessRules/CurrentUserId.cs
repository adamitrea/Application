using Application_DbAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Application_BusinessRules
{
    public interface ICurrentUserService
    {
        string GetCurrentUserId(IHttpContextAccessor httpContextAccessor);
    }

}

