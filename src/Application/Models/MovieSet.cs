using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Application.Models
{
    public class MovieSet
    {
        public int MovieSetID { get; set; }
        public string UserID { get; set; }
        public string SetName { get; set; }
        public string SetDescription { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreationDate { get; set; }

        public virtual ApplicationUser User { get; private set; }
        public virtual ICollection<MyMovie> MyMovies { get; set; }
    }
}
