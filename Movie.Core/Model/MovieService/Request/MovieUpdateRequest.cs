using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Movie.Core.Model.MovieService
{
    public class MovieUpdateRequest
    {
        [Required]
        public int MovieId { get; set; }

        [Required]
        [Range(1, 10)]
        public int Vote { get; set; }
        
        [StringLength(250, ErrorMessage = "Comment Can not Exceed 250 Characters")]
        public string Note { get; set; }
    }
}
