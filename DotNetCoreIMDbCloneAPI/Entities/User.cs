using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreIMDbCloneAPI.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string Role { get; set; }

        public virtual ICollection<Rating> Ratings { get; set; }
    }
}
