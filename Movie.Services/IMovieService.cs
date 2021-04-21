using Movie.Core.Model.MovieService;
using Movie.Core.Model.MovieService.Request;
using Movie.Core.Model.MovieService.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Services
{
    public interface IMovieService
    {
        public LoginResponse Login(GrantInfo authInfo);
        public MovieDetailResponse GetMovieDetail(int movieId);
        public MovieResponse GetMovieListPage(int specificPage);
        public MovieResponse GetMovieList(int pageCount);
        public MovieUpdateResponse UpdateMovie(MovieUpdateRequest movieUpdateRequest);
        public Task<bool> RecommendMovie(RecommendationRequest movieUpdateRequest);
        public GuestSessionResponse GetSession(string apiKey);
    }
}
