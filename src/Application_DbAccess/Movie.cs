using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Application_DbAccess
{
    public class Movie
    {
        public int MovieID { get; set; }
        public int TMDb { get; set; }
        public string MovieName { get; set; }
        public string MovieYear { get; set; }
        public string MovieDescription { get; set; }
        public string MovieIcon { get; set; }
        public double MovieRating { get; set; }
        public string MovieGenre { get; set; }

        public virtual ICollection<MyMovie> MyMovies { get; set; }

    }
}
