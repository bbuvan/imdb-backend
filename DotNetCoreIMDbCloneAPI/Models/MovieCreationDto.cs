using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreIMDbCloneAPI.Models
{
    public class MovieCreationDto
    {
        public string Name { get; set; }
        public DateTime ReleaseYear { get; set; }
        public int Runtime { get; set; }
        public string Summary { get; set; }
        public string ImageUrl { get; set; }
        public string Genres { get; set; }
        public string Actors { get; set; }
    }
}
