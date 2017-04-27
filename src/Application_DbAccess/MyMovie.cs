using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Application_DbAccess
{
    public class MyMovie
    {
        public int MyMovieID { get; set; }
        public int MovieSetID { get; set; }
        public int MovieID { get; set; }
        public double MyMovieRating { get; set; }
        public string MyMovieComment { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreationDate { get; set; }

        public virtual MovieSet MovieSet { get; set; }
        public virtual Movie Movie { get; set; }

    }
}
