using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movie.Core.Model.MovieService
{
    public class AppConfiguration
    {
        public AuthInfo AuthInfo { get; set; }
        public MovieService MovieService { get; set; }
        public SmtpInfo SmtpInfo { get; set; }
        public string FilePath { get; set; }
    }
    public class AuthInfo
    {
        public string token_url { get; set; }
        public GrantInfo GrantInfo { get; set; }
    }

    public class SmtpInfo 
    {
        public string smtp { get; set; }
        public string port { get; set; }
        public string from { get; set; }
        public string subject { get; set; }
    }
    public class GrantInfo
    {
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string audience { get; set; }
        public string grant_type { get; set; }
    }
    public class MovieService
    {
        public string api_endpoint { get; set; }
        public string api_key { get; set; }
    }
}
