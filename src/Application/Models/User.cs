using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string UserName { get; set; }

        public ICollection<MovieSet> MovieLists { get; set; }

    }
}
