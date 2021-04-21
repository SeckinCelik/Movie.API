using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Services
{
    public interface IEmailService
    {
        public Task<bool> Send(string body, string subject, IEnumerable<string> toList);
    }
}
