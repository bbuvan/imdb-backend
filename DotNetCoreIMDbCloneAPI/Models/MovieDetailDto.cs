using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

	
using AutoMapper;
namespace DotNetCoreIMDbCloneAPI.Models.DTOs
{
    public class MovieDetailDto
    {
        //public Movie Movie { get; set; }
        //  public string FirstName { get; set; }
        public int MovieId { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public double Rating { get; set; }
        public DateTime ReleaseYear { get; set; }
        public String ImageUrl { get; set; }
        public int Runtime { get; set; }
        public bool Watched { get; set; }
        public IEnumerable<Actor> Actors { get; set; }
        public IEnumerable<Genre> Genres { get; set; }

    }
}
