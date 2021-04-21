using System;
using System.Collections.Generic;
using System.Text;

namespace Movie.Core.Model.Data
{
    public class VoteData
    {
        public int MovieId { get; set; }
        public int Vote { get; set; }
        public string Comment { get; set; }
        public string Key { get; set; }
    }
}
