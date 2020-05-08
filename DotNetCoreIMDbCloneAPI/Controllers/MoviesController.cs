using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DotNetCoreIMDbCloneAPI.Models;
using DotNetCoreIMDbCloneAPI.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNetCoreIMDbCloneAPI.Helpers;
using DotNetCoreIMDbCloneAPI.Services;
using System.Net;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DotNetCoreIMDbCloneAPI.Controllers
{
    
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class MoviesController : Controller
    {
        private readonly IMovieDbRepository movieDbRepository;

        public MoviesController(IMovieDbRepository movieDbRepository)
        {
            this.movieDbRepository = movieDbRepository;
        }
        
        [HttpGet]
        public MovieListDTO GetMovies(int page = 0, string orderBy = "", string name = "")
        {
            orderBy = WebUtility.UrlDecode(orderBy);
            name = WebUtility.UrlDecode(name);

            if (orderBy.StartsWith("Rating"))
            {
                if (orderBy.EndsWith("DESC"))
                {
                    var movies = movieDbRepository.GetMoviesByRatingDescending(page, name)
                            .Select(x => new MovieDetailDto() { MovieId = x.MovieId, Name = x.Name, Summary = x.Summary, Actors = x.MovieActors.Select(y => y.Actor), Genres = x.MovieGenres.Select(y => y.Genre), ReleaseYear = x.ReleaseYear, Rating = x.Ratings.Count > 0 ? x.Ratings.Average(y => y.Score) : -1, Watched = false, ImageUrl = x.ImageUrl, Runtime = x.Runtime });
                    var data = new MovieListDTO { Data = movies, Page = page, Total = movies.Count() };
                    return data;
                }
                else
                {
                    var movies = movieDbRepository.GetMoviesByRatingAscending(page, name)
                            .Select(x => new MovieDetailDto() { MovieId = x.MovieId, Name = x.Name, Summary = x.Summary, Actors = x.MovieActors.Select(y => y.Actor), Genres = x.MovieGenres.Select(y => y.Genre), ReleaseYear = x.ReleaseYear, Rating = x.Ratings.Count > 0 ? x.Ratings.Average(y => y.Score) : -1, Watched = false, ImageUrl = x.ImageUrl, Runtime = x.Runtime });
                    var data = new MovieListDTO { Data = movies, Page = page, Total = movies.Count() };
                    return data;
                }
 
            }
            else
            {
                var movies = movieDbRepository.GetMovies(orderBy, page, name)
                                .Select(x => new MovieDetailDto() { MovieId = x.MovieId, Name = x.Name, Summary = x.Summary, Actors = x.MovieActors.Select(y => y.Actor), Genres = x.MovieGenres.Select(y => y.Genre), ReleaseYear = x.ReleaseYear, Rating = x.Ratings.Count > 0 ? x.Ratings.Average(y => y.Score) : -1, Watched = false, ImageUrl = x.ImageUrl, Runtime = x.Runtime });


                var data = new MovieListDTO { Data = movies, Page = page, Total = movies.Count() };

                return data;
            }
        }
        
        /*
        [HttpGet]
        public IActionResult GetMovies(int page = 0, string orderBy = "", string name = "")
        {
            var data = movieDbRepository.GetMovies(orderBy, page, name);
            return Ok(data);
        }
        */
        [HttpGet("{id}")]
        public MovieDetailDto GetMovie(int id)
        {
            var movie = movieDbRepository.GetMovieDetailed(id);

            var movieDetails = new MovieDetailDto
            {
                Actors = movie.MovieActors.Select(x => x.Actor),
                Genres = movie.MovieGenres.Select(x => x.Genre),
                Name = movie.Name,
                MovieId = movie.MovieId,
                Rating = movie.Ratings.Count > 0 ? movie.Ratings.Average(x => x.Score) : -1,
                Summary = movie.Summary,
                Runtime = movie.Runtime,
                ReleaseYear = movie.ReleaseYear,
                ImageUrl = movie.ImageUrl
                
            };

            return movieDetails;
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult DeleteMovie(int id)
        {
            var movie = movieDbRepository.GetMovieDetailed(id);

            if (movie == null)
                return NotFound();

            movieDbRepository.DeleteMovie(movie);

            return NoContent();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult PostMovie([FromBody] MovieCreationDto movieCreationDto)
        {

            Movie movie = new Movie();
            movie.Name = movieCreationDto.Name;
            movie.ReleaseYear = movieCreationDto.ReleaseYear;
            movie.Runtime = movieCreationDto.Runtime;
            movie.Summary = movieCreationDto.Summary;
            movie.ImageUrl = movieCreationDto.ImageUrl;

            Movie createdMovie = movieDbRepository.AddMovie(movie);
            try
            {
                if (movieCreationDto.Genres != null)
                    foreach (string id in movieCreationDto.Genres.Split(','))
                    {
                        MovieGenre movieGenre = new MovieGenre() { GenreId = int.Parse(id), MovieId = createdMovie.MovieId };
                        movieDbRepository.AddGenreToMovie(movieGenre);
                    }
            }
            catch (Exception)
            {

            }

            try
            {
                if (movieCreationDto.Actors != null)
                    foreach (string id in movieCreationDto.Actors.Split(','))
                    {
                        MovieActor movieActor = new MovieActor() { ActorId = int.Parse(id), MovieId = createdMovie.MovieId };
                        movieDbRepository.AddActorToMovie(movieActor);
                    }
            }
            catch (Exception)
            {

               
            }
            

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult UpdateMovie(int id, [FromBody] MovieCreationDto movieCreationDto)
        {

            Movie movie = new Movie();
            movie.Name = movieCreationDto.Name;
            movie.ReleaseYear = movieCreationDto.ReleaseYear;
            movie.Runtime = movieCreationDto.Runtime;
            movie.Summary = movieCreationDto.Summary;
            movie.ImageUrl = movieCreationDto.ImageUrl;
            movie.MovieId = id;

            movieDbRepository.UpdateMovie(movie);

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("{id}/actor")]
        public IActionResult PostMovieActor(int id, [FromBody]MovieActor movieActor)
        {
            movieDbRepository.AddActorToMovie(new MovieActor { ActorId = movieActor.ActorId, MovieId = id });

            return Ok();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("{id}/genre")]
        public IActionResult PostMovieGenre(int id, [FromBody]MovieGenre movieGenre)
        {
            movieDbRepository.AddGenreToMovie(new MovieGenre { GenreId = movieGenre.GenreId, MovieId = id });

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}/actor/{actorId}")]
        public IActionResult DeleteMovieActor(int id, int actorId)
        {
            var movieActor = movieDbRepository.GetMovieActor(id, actorId);

            if (movieActor == null)
                return NotFound();

            movieDbRepository.DeleteActorFromMovie(movieActor);

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}/genre/{genreId}")]
        public IActionResult DeleteMovieGenre(int id, int genreId)
        {
            var movieGenre = movieDbRepository.GetMovieGenre(id, genreId);

            if (movieGenre == null)
                return NotFound();

            movieDbRepository.DeleteGenreFromMovie(movieGenre);

            return NoContent();
        }

        [HttpPost("{id}/rate")]
        public IActionResult RateMovie(int id, [FromBody]Rating rating)
        {
           var userRating = movieDbRepository.GetMovieRating(id, int.Parse(this.User.Claims.First().Value));
            if(userRating != null)
            {
                movieDbRepository.DeleteRating(userRating);
            }

            movieDbRepository.RateMovie(new Rating { MovieId = id, Score = rating.Score, UserId = int.Parse(this.User.Claims.First().Value) });
            return Ok(rating.Score);
        }
    }
}
