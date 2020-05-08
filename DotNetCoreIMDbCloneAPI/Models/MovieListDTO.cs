using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreIMDbCloneAPI.Models.DTOs
{
    public class MovieListDTO
    {
        /*
        public int MovieId { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public IEnumerable<Actor> Actors { get; set; }
        public IEnumerable<Genre> Genres { get; set; }
        */
        public IEnumerable<MovieDetailDto> Data{ get; set; }
        public int Total { get; set; }
        public int Page { get; set; }
    }
}
