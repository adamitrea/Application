using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Application_DbAccess
{
    // Add profile data for application users by adding properties to the User class
    public class ApplicationUser: IdentityUser
    {

        public ICollection<MovieSet> MovieSets { get; set; }
    }
}
