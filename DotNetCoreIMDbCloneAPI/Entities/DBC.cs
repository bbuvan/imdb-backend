using DotNetCoreIMDbCloneAPI.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreIMDbCloneAPI.Models
{
    public class DBC : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<MovieGenre> MovieGenres { get; set; }
        public DbSet<MovieActor> MovieActors { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<WatchLater> WatchLaters{ get; set; }
        public DBC(DbContextOptions<DBC> options)
            : base(options)
        {

        }
/*
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
        */
    }
}
