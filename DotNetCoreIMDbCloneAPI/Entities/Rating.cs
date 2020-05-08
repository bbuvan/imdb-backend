using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreIMDbCloneAPI.Models
{
    public class Rating
    {
        public int RatingId { get; set; }
        public int MovieId { get; set; }
        public double Score { get; set; }
        public int UserId { get; set; }
    }
}
