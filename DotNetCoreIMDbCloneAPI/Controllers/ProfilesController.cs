using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCoreIMDbCloneAPI.Entities;
using DotNetCoreIMDbCloneAPI.Helpers;
using DotNetCoreIMDbCloneAPI.Models.DTOs;
using DotNetCoreIMDbCloneAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DotNetCoreIMDbCloneAPI.Controllers
{
    [Authorize]
    [Route("api/me")]
    public class ProfilesController : Controller
    {
        private readonly IMovieDbRepository movieDbRepository;

        public ProfilesController(IMovieDbRepository movieDbRepository)
        {
            this.movieDbRepository = movieDbRepository;
        }

        [HttpGet("watchlaters")]
        public MovieListDTO Get()
        {
            var movies = movieDbRepository.GetWatchLaterByUserId(int.Parse(this.User.Claims.First().Value))
                             .Select(x => new MovieDetailDto() { MovieId = x.MovieId, Name = x.Name, Summary = x.Summary, Actors = x.MovieActors.Select(y => y.Actor), Genres = x.MovieGenres.Select(y => y.Genre), ReleaseYear = x.ReleaseYear, Rating = x.Ratings.Average(y => y.Score), Watched = x.WatchLaters.Any(y => y.MovieId == x.MovieId), ImageUrl = x.ImageUrl });

            var data = new MovieListDTO { Data = movies };

            return data;
        }

        [HttpPost("watchlaters")]
        public IActionResult PostWatchLater([FromBody] WatchLater watchLater)
        {
            movieDbRepository.AddToWatchLater(new WatchLater { MovieId = watchLater.MovieId, UserId = int.Parse(this.User.Claims.First().Value) });
            
            return Ok();
        }

        [HttpDelete("watchlaters/{id}")]
        public IActionResult DeleteWatchLater(int id)
        {
            movieDbRepository.DeleteWatchLater(int.Parse(this.User.Claims.First().Value) ,id);

            return Ok();
        }

    }
}
