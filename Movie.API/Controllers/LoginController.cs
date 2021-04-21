using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Movie.Core.Model.MovieService;
using Movie.Core.Model.MovieService.Response;
using Movie.Services;
using RestSharp;

namespace Movie.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        AppConfiguration _appConfiguration;
        IMovieService _movieService;
        private readonly ILogger _logger;

        public LoginController(AppConfiguration appConfiguration, IMovieService movieService, ILogger<LoginController> logger)
        {
            _appConfiguration = appConfiguration;
            _movieService = movieService;
            _logger = logger;
        }

        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        public IActionResult Login([FromForm] GrantInfo authInfo)
        {
            try
            {
                return Ok(_movieService.Login(authInfo));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500);
            }
        }
    }
}
