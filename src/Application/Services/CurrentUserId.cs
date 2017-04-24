using Application.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface ICurrentUserId
    {
        string GetCurrentUserId(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager);
    }

    public class CurrentUserId : ICurrentUserId
    {
        public string GetCurrentUserId(IHttpContextAccessor _httpContextAccessor, UserManager<ApplicationUser> _userManager)
        {
            var userId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);

            return userId;
        }
    }


}
