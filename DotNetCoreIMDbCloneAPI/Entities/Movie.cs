using DotNetCoreIMDbCloneAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DotNetCoreIMDbCloneAPI.Models
{
    public class Movie
    {
        public int MovieId { get; set; }
        public string Name { get; set; }
        public DateTime ReleaseYear { get; set; }
        public int Runtime { get; set; }
        public string Summary { get; set; }
        public string ImageUrl { get; set; }
        public virtual ICollection<MovieGenre> MovieGenres { get; set; }
        public virtual ICollection<MovieActor> MovieActors { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<WatchLater> WatchLaters { get; set; }
    }
}
