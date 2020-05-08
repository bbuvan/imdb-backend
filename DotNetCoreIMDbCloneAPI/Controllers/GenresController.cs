using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCoreIMDbCloneAPI.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DotNetCoreIMDbCloneAPI.Controllers
{
    [Route("api/[controller]")]
    public class GenresController : Controller
    {
        private readonly IMovieDbRepository movieDbRepository;

        public GenresController(IMovieDbRepository movieDbRepository)
        {
            this.movieDbRepository = movieDbRepository;
        }

        [HttpGet]
        public IActionResult GetGenres()
        {
            return Ok(movieDbRepository.GetGenres());
        }
    }
}
