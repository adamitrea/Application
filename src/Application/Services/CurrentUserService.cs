using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Models;
using Application_BusinessRules;
using Application_DbAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{

    public class CurrentUserService : ICurrentUserService
    {
        private readonly UserManager<ApplicationUser> userManager;
        
        public CurrentUserService(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public string GetCurrentUserId(IHttpContextAccessor _httpContextAccessor)
        {
            var userId = userManager.GetUserId(_httpContextAccessor.HttpContext.User);

            return userId;
        }
    }
}
