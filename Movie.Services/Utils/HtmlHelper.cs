using Movie.Core.Model.MovieService;
using System;
using System.Collections.Generic;
using System.Text;

namespace Movie.Services.Utils
{
    public class HtmlHelper
    {
        public static string ConvertToHtml(MovieDetailResponse movieDetailResponse)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<h1>" + movieDetailResponse.original_title + "</h1>");
            sb.Append("<p>" + movieDetailResponse.original_title + " is recommended to you !!!</p>");

            return sb.ToString();
        }
    }
}
