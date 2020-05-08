using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreIMDbCloneAPI.Models.DTOs
{
    public class ActorDetailDto
    {
        public int ActorId { get; set; }
        public string FullName { get; set; }
        public string ImageUrl { get; set; }
        // public IEnumerable<Movie> Movies { get; set; }
        public MovieListDTO Movies { get; set; }
    }
}
