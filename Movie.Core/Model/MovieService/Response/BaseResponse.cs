using Movie.Core.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Movie.Core.Model.MovieService.Response
{
    public class BaseResponse<T> where T : class
    {
        public T Data { get; set; }
        public string Message { get; set; }
        public ErrorCode ErrorCode { get; set; }
    }
}
