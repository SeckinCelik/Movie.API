using System;
using System.Collections.Generic;
using System.Text;

namespace Movie.Core.Model.MovieService.Response
{
    public class MovieUpdateResponse
    {
        public bool success { get; set; }
        public int status_code { get; set; }
        public string status_message { get; set; }

    }
}
