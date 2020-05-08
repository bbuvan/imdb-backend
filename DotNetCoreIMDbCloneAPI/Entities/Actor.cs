using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DotNetCoreIMDbCloneAPI.Models
{
    public class Actor
    {
        public int ActorId { get; set; }
        public string FullName { get; set; }
        public string ImageUrl { get; set; }
        [JsonIgnore]
        public virtual ICollection<MovieActor> MovieActors { get; set; }
    }
}
