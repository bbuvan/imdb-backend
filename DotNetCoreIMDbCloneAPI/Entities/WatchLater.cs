using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreIMDbCloneAPI.Entities
{
    public class WatchLater
    {
        public int WatchLaterId { get; set; }
        public int MovieId { get; set; }
        public int UserId { get; set; }
    }
}
