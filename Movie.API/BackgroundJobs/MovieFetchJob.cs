using Microsoft.Extensions.Logging;
using Movie.Core.Model.MovieService;
using Movie.Services;
using Quartz;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movie.API.BackgroundJobs
{
    [DisallowConcurrentExecution]
    public class MovieFetchJob : IJob
    {
        private readonly ILogger<MovieFetchJob> _logger;
        IMovieService _movieService;
        public MovieFetchJob(ILogger<MovieFetchJob> logger, IMovieService movieService)
        {
            _logger = logger;
            _movieService = movieService;
        }
        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                int currentPage = 1;               
                var response = _movieService.GetMovieListPage(currentPage);

                MovieResponse movieResponse = new MovieResponse();
                movieResponse.total_pages = response.total_pages;
                movieResponse.page = currentPage;
                movieResponse.results = response.results;

                for (int i = 2; i < response.total_pages; i++)
                {
                    movieResponse.results.AddRange(_movieService.GetMovieListPage(i).results);
                }
                movieResponse.total_results = movieResponse.results.Count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return Task.CompletedTask;
        }
    }
}
