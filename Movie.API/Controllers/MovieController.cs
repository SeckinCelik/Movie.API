using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Movie.Core.Model.MovieService;
using Movie.Core.Model.MovieService.Request;
using Movie.Services;
using RestSharp;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;

namespace Movie.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        AppConfiguration _appConfiguration;
        IMovieService _movieService;
        ILogger<MovieController> _logger;
        public MovieController(AppConfiguration appConfiguration, IMovieService movieService, ILogger<MovieController> logger)
        {
            _appConfiguration = appConfiguration;
            _movieService = movieService;
            _logger = logger;
        }

        [HttpGet("list-movies-page")]
        [Authorize("read:movies")]
        public IActionResult GetMovieListPage([FromQuery,Required]int specificPage )
        {
            try
            {
                return Ok(_movieService.GetMovieListPage(specificPage));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("list-movies")]
        [Authorize("read:movies")]
        public IActionResult GetMovieList([FromQuery,Required]int pageCount)
        {
            try
            {
                return Ok(_movieService.GetMovieList(pageCount));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("movie-detail")]
        [Authorize("read:movies")]
        public IActionResult GetMovieDetail([FromQuery, Required] int movieid)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState.Values);

                return Ok(_movieService.GetMovieDetail(movieid));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("rate-movie")]
        [Authorize("write:movies")]
        public IActionResult UpdateMovie([FromBody] MovieUpdateRequest updateRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState.Values);

                return Ok(_movieService.UpdateMovie(updateRequest));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("recommend-movie")]
        [Authorize("read:movies")]
        public async Task<IActionResult> RecommendMovie([FromBody] RecommendationRequest recommendRquest)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState.Values);
                bool isSent = await _movieService.RecommendMovie(recommendRquest);
                return Ok(isSent);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
