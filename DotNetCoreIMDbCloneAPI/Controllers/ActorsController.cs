using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCoreIMDbCloneAPI.Models;
using DotNetCoreIMDbCloneAPI.Models.DTOs;
using DotNetCoreIMDbCloneAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DotNetCoreIMDbCloneAPI.Controllers
{
    [Route("api/[controller]")]
    public class ActorsController : Controller
    {
        private readonly IMovieDbRepository _movieDbRepository;

        public ActorsController(IMovieDbRepository movieDbRepository)
        {
            _movieDbRepository = movieDbRepository;
        }

        [HttpGet("{id}/detail")]
        public ActorDetailDto GetDetail(int id)
        {
            var actor = _movieDbRepository.GetActor(id);
            var actorMovies = _movieDbRepository.GetMoviesByActorId(id);

            var movieDetails = actorMovies.Select(x => new MovieDetailDto() { MovieId = x.MovieId, Name = x.Name, Summary = x.Summary, Actors = x.MovieActors.Select(y => y.Actor), Genres = x.MovieGenres.Select(y => y.Genre), Rating = x.Ratings.Count > 0 ? x.Ratings.Average(y => y.Score) : -1, ImageUrl = x.ImageUrl });


            return new ActorDetailDto
            {
                ActorId = id,
                FullName = actor.FullName,
                ImageUrl = actor.ImageUrl,
                Movies = new MovieListDTO() { Data = movieDetails, Total = movieDetails.Count() }
            };

        }

        [HttpGet]
        public IActionResult GetActors(string name = "", int page = 0)
        {
            var actors = _movieDbRepository.GetActors(name, page);

            if (actors == null)
                return NotFound();

            return Ok(actors);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var actor = _movieDbRepository.GetActor(id);

            if (actor == null)
                return NotFound();

            return Ok(actor);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public Actor Post([FromBody] Actor actor)
        {
            return _movieDbRepository.AddActor(actor);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Actor actorData)
        {
            var actor = _movieDbRepository.GetActor(id);
            if (actor == null)
                return NotFound();

            actorData.ActorId = id;
            _movieDbRepository.UpdateActor(actorData);

            return Ok(actorData);

        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var actor = _movieDbRepository.GetActor(id);
            if (actor == null)
                return NotFound();

            _movieDbRepository.DeleteActor(actor);

            return NoContent();

        }

     
    }
}
