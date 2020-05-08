using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DotNetCoreIMDbCloneAPI.Models
{
    public class MovieActor
    {
        public int MovieActorId { get; set; }
        public int MovieId { get; set; }
        public int ActorId { get; set; }
        public virtual Actor Actor { get; set; }

       // public virtual Movie Movie { get; set; }
    }
}
