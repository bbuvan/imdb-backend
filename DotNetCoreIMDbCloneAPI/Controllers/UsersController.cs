using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DotNetCoreIMDbCloneAPI.Models.DTOs;
using DotNetCoreIMDbCloneAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DotNetCoreIMDbCloneAPI.Controllers
{
   // [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly IMovieDbRepository movieDbRepository;

        public UsersController(IMovieDbRepository movieDbRepository)
        {
            this.movieDbRepository = movieDbRepository;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]AuthenticationDto model)
        {

            //  var user = _userService.Authenticate(model.Username, model.Password);

            var token = movieDbRepository.Login(model.Username, model.Password);

            if (token == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]AuthenticationDto model)
        {
            var token = movieDbRepository.Register(model.Username, model.Password);

            if (token == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(token);
        }

    }
}
