using DotNetCoreIMDbCloneAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCoreIMDbCloneAPI.Helpers;
using DotNetCoreIMDbCloneAPI.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace DotNetCoreIMDbCloneAPI.Services
{
    public class MovieDbRepository : IMovieDbRepository
    {
        private readonly DBC _context;
        public MovieDbRepository(DBC context)
        {
            _context = context;
        }

        public string Register(string username, string password)
        {
            string salt = Salt.Create();
            string hash = Hash.Create(password, salt);

            _context.Users.Add(new User { Username = username, PasswordHash = hash, PasswordSalt = salt });
            _context.SaveChanges();

            return Login(username, password);
        }

        public string Login(string username, string password)
        {
            var user = _context.Users.SingleOrDefault(x => x.Username == username);

            if (user == null)
                return null;

            string hash = Hash.Create(password, user.PasswordSalt);

            if (Hash.Validate(password, user.PasswordSalt, user.PasswordHash))
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("secretsecretsecretsecretsecretsecret");
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, user.UserId.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }

            return null;
        }

        public Actor AddActor(Actor actor)
        {
            actor = _context.Actors.Add(actor).Entity;
            _context.SaveChanges();
            return actor;
        }

        public void AddActorToMovie(MovieActor movieActor)
        {
            _context.MovieActors.Add(movieActor);
            _context.SaveChanges();
        }

        public void AddGenreToMovie(MovieGenre movieGenre)
        {
            _context.MovieGenres.Add(movieGenre);
            _context.SaveChanges();
        }

        public Movie AddMovie(Movie movie)
        {
            movie = _context.Movies.Add(movie).Entity;
            _context.SaveChanges();
            return movie;
        }

        public void AddToWatchLater(WatchLater watchLater)
        {
            _context.WatchLaters.Add(watchLater);
            _context.SaveChanges();
        }

        public void DeleteActor(Actor actor)
        {
            _context.Remove(actor);
            _context.SaveChanges();
        }

        public void DeleteActorFromMovie(MovieActor movieActor)
        {
            _context.Remove(movieActor);
            _context.SaveChanges();
        }

        public Actor GetActor(int id)
        {
            return _context.Actors.Where(x => x.ActorId == id).FirstOrDefault();
            // .Include("MovieActors.Movie").FirstOrDefault();
        }

        public MovieActor GetMovieActor(int movieId, int actorId)
        {
            return _context.MovieActors.FirstOrDefault(x => x.MovieId == movieId && x.ActorId == actorId);
        }

        public IEnumerable<Movie> GetMovies(string orderBy, int page = 0, string name= "")
        {
            return _context.Movies
                .Where(x => x.Name.StartsWith(name))
                  
                // .Include("Ratings")
                .OrderBy(orderBy)
               //.Take(1);
                 .Skip(page * 10).Take(10)
                 .Include("Ratings")
                 .Include("MovieActors.Actor")
                  .Include("MovieGenres.Genre")
                  ;
        }

        public IEnumerable<Movie> GetMoviesByGenreId(int genreId,string orderBy, int page = 0)
        {
            return _context.Movies
                .Where(x => x.MovieGenres.Any(y=> y.GenreId == genreId))
                  .Include(x => ((MovieActor)x.MovieActors).Actor)
                  .Include("MovieGenres.Genre")
                 .Include("Ratings")
                .OrderBy(orderBy)
                 .Skip(page * 10).Take(10);
        }

        public IEnumerable<Movie> GetMoviesByActorId(int id)
        {
            return _context.Movies
                .Where(x => x.MovieActors.Any(y => y.ActorId == id))
                 .Include(x => ((MovieActor)x.MovieActors).Actor)
                 .Include("MovieGenres.Genre")
                .Include("Ratings");
        }

        public IEnumerable<Movie> GetMoviesByRatingDescending(int page = 0, string name = "")
        {
            return _context.Movies
                .Where(x=> x.Name.StartsWith(name))
                
               .Include("Ratings")
               .OrderByDescending(x => x.Ratings.Average(y => y.Score))
               .Skip(page * 10).Take(10)
               .Include(x => ((MovieActor)x.MovieActors).Actor)
                .Include("MovieGenres.Genre");
        }

        public IEnumerable<Movie> GetMoviesByRatingAscending(int page = 0, string name = "")
        {
            return _context.Movies
                .Where(x=> x.Name.StartsWith(name))
                
               .Include("Ratings")
               .OrderBy(x => x.Ratings.Average(y => y.Score))
               .Skip(page * 10).Take(10)
               .Include(x => ((MovieActor)x.MovieActors).Actor)
                .Include("MovieGenres.Genre");
        }

        public IEnumerable<Movie> GetWatchLaterByUserId(int userId)
        {
            return _context.Movies
               .Include(x => ((MovieActor)x.MovieActors).Actor)
               .Include("MovieGenres.Genre")
              .Include("Ratings")
                .Include("WatchLaters")
                .Where(x => x.WatchLaters.Any(y => y.UserId == userId));
        }

        public Actor UpdateActor(Actor actor)
        {
            var _actor = _context.Actors.FirstOrDefault(x => x.ActorId == actor.ActorId);

            _context.Entry(_actor).CurrentValues.SetValues(actor);
            _context.SaveChanges();

            return actor;
        }

        public void UpdateMovie(Movie movie)
        {
            var _movie = _context.Movies.FirstOrDefault(x => x.MovieId == movie.MovieId);

            _context.Entry(_movie).CurrentValues.SetValues(movie);
            _context.SaveChanges();
        }

        public Movie GetMovieDetailed(int id)
        {
            return _context.Movies.Where(x => x.MovieId == id)
                .Include(x => ((MovieActor)x.MovieActors).Actor)
               .Include("MovieGenres.Genre")
               .Include("Ratings")
               .FirstOrDefault();
        }

        public void DeleteMovie(Movie movie)
        {
            _context.Remove(movie);
            _context.SaveChanges();
        }

        public void DeleteGenreFromMovie(MovieGenre movieGenre)
        {
            _context.Remove(movieGenre);
            _context.SaveChanges();
        }

        public MovieGenre GetMovieGenre(int movieId, int genreId)
        {
            return _context.MovieGenres.FirstOrDefault(x => x.MovieId == movieId && x.GenreId == genreId);
        }

        public IEnumerable<Actor> GetActors(string name = "", int page = 0)
        {
            return _context.Actors
                .Where(x => x.FullName.StartsWith(name))
               .Skip(page * 10).Take(10);
        }

        public IEnumerable<Genre> GetGenres()
        {
            return _context.Genres;
        }

        public void DeleteWatchLater(int userId, int movieId)
        {
            var watchLater = _context.WatchLaters.FirstOrDefault(x => x.UserId == userId && x.MovieId == movieId);
            _context.WatchLaters.Remove(watchLater);
            _context.SaveChanges();
        }

        public void RateMovie(Rating rating)
        {
            rating = _context.Ratings.Add(rating).Entity;
            _context.SaveChanges();

        }

        public void DeleteRating(Rating rating)
        {
            _context.Remove(rating);
            _context.SaveChanges();
        }

        public Rating GetMovieRating(int movieId, int userId)
        {
            return _context.Ratings.Where(x => x.MovieId == movieId && x.UserId == userId).FirstOrDefault();
        }
    }
}
