using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreIMDbCloneAPI.Models
{
    public class MovieGenre
    {
        public int MovieGenreId { get; set; }
        public int MovieId { get; set; }
        public int GenreId { get; set; }
        public virtual Genre Genre { get; set; }
    }
    
}
