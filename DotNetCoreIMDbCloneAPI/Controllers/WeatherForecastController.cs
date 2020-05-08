using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DotNetCoreIMDbCloneAPI.Models;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DotNetCoreIMDbCloneAPI.Controllers
{
    [ApiController]
    
    public class WeatherForecastController : ControllerBase
    {
        private DBC _context { get; set; }
        public WeatherForecastController(DBC context)
        {
            _context = context;
        }
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };


        [HttpGet]
        [Route("api/[controller]/u")]
        public async Task Get()
        {/*

            var genre = new Genre { Name = "z" };
            _context.Genres.Add(genre);
            // _context.SaveChanges();
            _context.Entry(genre).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            var count = _context.ChangeTracker.Entries().Where(
           // 
           */

            /*
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
            */


            HttpClient client = new HttpClient();

            var response = await client.GetAsync("https://m.imdb.com/chart/top/?ref_=nv_mv_250");
            var pageContents = await response.Content.ReadAsStringAsync();
            HtmlDocument pageDocument = new HtmlDocument();
            pageDocument.LoadHtml(pageContents);

            var doc = pageDocument.DocumentNode.SelectNodes("//a[@class='btn-full']");
            foreach (var item2 in doc)
            {
                var movieUrl = item2.GetAttributeValue("href", string.Empty);
                response = await client.GetAsync($"https://m.imdb.com/{ movieUrl}");
                pageContents = await response.Content.ReadAsStringAsync();

                pageDocument = new HtmlDocument();
                pageDocument.LoadHtml(pageContents);
                doc = pageDocument.DocumentNode.SelectNodes("//script[@type='application/ld+json']");
                dynamic x = JsonConvert.DeserializeObject(doc[0].InnerText);

                // Console.WriteLine("Name: " + x.name);
                string movieName = x.name;
                Movie movie = _context.Movies.Where(movie => movie.Name == movieName).FirstOrDefault();
                if(movie == null)
                {
                    var duration = pageDocument.DocumentNode.SelectNodes("//time[@itemprop='duration']");
                     //Console.WriteLine("\n" + doc[0].InnerText.Trim().Split(" ")[0]);
                    movie = new Movie { Name = movieName, Runtime=int.Parse(duration[0].InnerText.Trim().Split(" ")[0]),Summary=x.description, ReleaseYear = x.datePublished, ImageUrl = x.image };
                    _context.Movies.Add(movie);
                    _context.SaveChanges();
                }
                x.genre = new JArray(x.genre);
               // Console.Write("Genres: ");
                foreach (string item in x.genre)
                {
                    Genre genre= _context.Genres.Where(x => x.Name == item).FirstOrDefault();
                    if(genre == null)
                    {
                        genre = new Genre() { Name = item };
                        _context.Genres.Add(genre);
                        _context.SaveChanges();
                    }
                    MovieGenre movieGenre = new MovieGenre() { MovieId = movie.MovieId, GenreId = genre.GenreId };
                    _context.MovieGenres.Add(movieGenre);
                    _context.SaveChanges();
                    
                    //Console.Write(item);
                }
                x.actor = new JArray(x.actor);
               // Console.Write("\nActors: ");
                foreach (var item in x.actor)
                {
                    string nm = item.name;
                    Actor actor = _context.Actors.Where(x => x.FullName == nm).FirstOrDefault();
                    if (actor == null)
                    {
                        actor = new Actor() { FullName = nm };
                        _context.Actors.Add(actor);
                        _context.SaveChanges();
                    }

                    MovieActor movieActor = new MovieActor() { MovieId = movie.MovieId, ActorId = actor.ActorId };
                    _context.MovieActors.Add(movieActor);
                    _context.SaveChanges();
                }

                _context.Ratings.Add(new Rating { MovieId = movie.MovieId, UserId = 1, Score = x.aggregateRating.ratingValue });
                _context.SaveChanges();

                x.director = new JArray(x.director);
               // Console.Write("\nDirector: ");
                foreach (var item in x.director)
                {
                    /*
                    Actor actor = _context.Actors.Where(x => x.FirstName == item).FirstOrDefault();
                    if (actor == null)
                    {
                        actor = new Actor() { Name = item };
                        _context.Genres.Add(actor);
                        _context.SaveChanges();
                    }
                    */
                }

              //  Console.Write("\nDescription: ");
               // Console.Write(x.description);

               // Console.Write("\nPublish: ");
               // Console.Write(x.datePublished);


                pageDocument = new HtmlDocument();
                pageDocument.LoadHtml(pageContents);
                doc = pageDocument.DocumentNode.SelectNodes("//time[@itemprop='duration']");
               // Console.WriteLine("\n" + doc[0].InnerText.Trim().Split(" ")[0]);
            }
        }

        [HttpGet, Route("[controller]")]
        public async Task Test()
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync("http://www.nzherald.co.nz/");
            var pageContents = await response.Content.ReadAsStringAsync();
            Console.WriteLine(pageContents);
            Console.ReadLine();
        }


    }
}
