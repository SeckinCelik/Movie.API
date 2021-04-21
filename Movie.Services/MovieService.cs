using Movie.Core.Constants;
using Movie.Core.Model.Data;
using Movie.Core.Model.MovieService;
using Movie.Core.Model.MovieService.Request;
using Movie.Core.Model.MovieService.Response;
using Movie.Services.Utils;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Linq;

namespace Movie.Services
{
    public class MovieService : IMovieService
    {
        AppConfiguration _appConfiguration;
        IEmailService _emailService;

        public MovieService(AppConfiguration appConfiguration, IEmailService emailService)
        {
            _appConfiguration = appConfiguration;
            _emailService = emailService;
        }

        public MovieDetailResponse GetMovieDetail(int movieId)
        {
            var response = GetMovieDetailResponse(movieId).Data;
            var userRatings = new FileManager(_appConfiguration.FilePath).ReadFile<VoteData>().LastOrDefault();

            if (userRatings != null)
            {
                response.user_vote = userRatings.Vote;
                response.user_comment = userRatings.Comment;
            }

            return response;
        }
        public LoginResponse Login(GrantInfo authInfo)
        {
            try
            {
                IRestClient client = new RestClient(_appConfiguration.AuthInfo.token_url);
                RestRequest request = new RestRequest(Method.POST);
                request.AddHeader("content-type", "application/json");
                request.AddParameter("application/json", Newtonsoft.Json.JsonConvert.SerializeObject(authInfo), ParameterType.RequestBody);
                var response = client.Post<LoginResponse>(request);
                return response.Data;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public MovieResponse GetMovieListPage(int specificPage)
        {
            try
            {
                IRestClient restClient = new RestClient(_appConfiguration.MovieService.api_endpoint);
                var request = new RestRequest("/3/discover/movie", Method.GET);
                request.AddQueryParameter("api_key", _appConfiguration.MovieService.api_key);
                request.AddQueryParameter("page", specificPage.ToString());

                var response = restClient.Get<MovieResponse>(request);
                return response.Data;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public MovieUpdateResponse UpdateMovie(MovieUpdateRequest movieUpdateRequest)
        {
            try
            {
                var session = GetSession(_appConfiguration.MovieService.api_key);

                IRestClient restClient = new RestClient(_appConfiguration.MovieService.api_endpoint);
                var request = new RestRequest("/3/movie/" + movieUpdateRequest.MovieId + "/rating", Method.POST);
                request.AddQueryParameter("api_key", _appConfiguration.MovieService.api_key);
                request.AddQueryParameter("guest_session_id", session.guest_session_id);
                request.RequestFormat = DataFormat.Json;

                request.AddJsonBody(new { value = movieUpdateRequest.Vote });

                var response = restClient.Post<MovieUpdateResponse>(request);

                if (response.Data != null & response.Data.success)
                {
                    new FileManager(_appConfiguration.FilePath).AppendToFile<VoteData>(
                        new VoteData
                        {
                            Vote = movieUpdateRequest.Vote,
                            Comment = movieUpdateRequest.Note,
                            MovieId = movieUpdateRequest.MovieId,
                            Key = session.guest_session_id
                        });
                }

                return response.Data;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> RecommendMovie(RecommendationRequest recommmendationRequest)
        {
            var response = GetMovieDetailResponse(recommmendationRequest.MovieId);

            if (response.Data != null)
            {
                string htmlMessage = HtmlHelper.ConvertToHtml(response.Data);
                bool isSent = await _emailService.Send(htmlMessage, _appConfiguration.SmtpInfo.subject, new List<string> { recommmendationRequest.Email });
                return isSent;
            }
            return false;
        }
        private IRestResponse<MovieDetailResponse> GetMovieDetailResponse(int movieId)
        {
            IRestClient restClient = new RestClient(_appConfiguration.MovieService.api_endpoint);
            var request = new RestRequest("/3/movie/" + movieId, Method.GET);
            request.AddQueryParameter("api_key", _appConfiguration.MovieService.api_key);
            request.AddQueryParameter("language", "en-US");
            return restClient.Get<MovieDetailResponse>(request);
        }
        public MovieResponse GetMovieList(int pageCount)
        {
            MovieResponse movieResponse = new MovieResponse();
            movieResponse.total_pages = pageCount;
            movieResponse.page = 1;
            movieResponse.results = new List<Core.Model.MovieService.Movie>();

            IRestClient restClient = new RestClient(_appConfiguration.MovieService.api_endpoint);
            var request = new RestRequest("/3/discover/movie", Method.GET);
            request.AddQueryParameter("api_key", _appConfiguration.MovieService.api_key);

            for (int i = 1; i <= pageCount; i++)
            {
                request.AddQueryParameter("page", i.ToString());
                var response = restClient.Get<MovieResponse>(request);

                if (response.Data != null)
                {
                    movieResponse.results.AddRange(response.Data.results);
                }
            }
            movieResponse.total_results = movieResponse.results.Count;

            return movieResponse;
        }
        public GuestSessionResponse GetSession(string apiKey)
        {
            IRestClient restClient = new RestClient(_appConfiguration.MovieService.api_endpoint);
            var request = new RestRequest("/3/authentication/guest_session/new", Method.GET);
            request.AddQueryParameter("api_key", _appConfiguration.MovieService.api_key);

            var response = restClient.Get<GuestSessionResponse>(request);

            return response.Data;
        }
    }
}
