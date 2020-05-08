using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DotNetCoreIMDbCloneAPI.Models
{
    public class Genre
    {
        public int GenreId { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        [JsonIgnore]
        public virtual ICollection<MovieGenre> MovieGenres { get; set; }
    }
}
