using DotNetCoreIMDbCloneAPI.Entities;
using DotNetCoreIMDbCloneAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreIMDbCloneAPI.Services
{
    public interface IMovieDbRepository
    {
        string Register(string username, string password);
        string Login(string username, string password);
        IEnumerable<Movie> GetMovies(string orderBy, int page = 0, string name = "");
        IEnumerable<Movie> GetMoviesByRatingDescending(int page = 0, string name = "");
        IEnumerable<Movie> GetMoviesByRatingAscending(int page = 0, string name = "");
        IEnumerable<Movie> GetMoviesByGenreId(int genreId, string orderBy, int page = 0);
        IEnumerable<Movie> GetMoviesByActorId(int id);
        Movie GetMovieDetailed(int id);
        Movie AddMovie(Movie movie);
        void UpdateMovie(Movie movie);
        void DeleteMovie(Movie movie);
        Actor GetActor(int id);
        IEnumerable<Actor> GetActors(string name = "", int page = 0);
        Actor AddActor(Actor actor);

        Actor UpdateActor(Actor actor);
        void DeleteActor(Actor actor);

        MovieActor GetMovieActor(int movieId, int actorId);
        void AddActorToMovie(MovieActor actor);
        void DeleteActorFromMovie(MovieActor movieActor);

        IEnumerable<Movie> GetWatchLaterByUserId(int userId);
        void AddToWatchLater(WatchLater watchLater);
        void DeleteWatchLater(int userId, int movieId);



        IEnumerable<Genre> GetGenres();
        MovieGenre GetMovieGenre(int movieId, int genreId);
        void AddGenreToMovie(MovieGenre movieGenre);
        void DeleteGenreFromMovie(MovieGenre movieGenre);

        void RateMovie(Rating rating);
        void DeleteRating(Rating rating);
        Rating GetMovieRating(int movieId, int userId);
    }
}
