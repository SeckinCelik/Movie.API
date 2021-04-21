using System;
using System.Collections.Generic;
using System.Text;

namespace Movie.Core.Model.MovieService.Response
{
    public class GuestSessionResponse
    {
        public bool success { get; set; }
        public string guest_session_id { get; set; }
        public string expires_at { get; set; }
    }
}
