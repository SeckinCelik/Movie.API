using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Movie.Core.Model.MovieService.Request
{
    public class RecommendationRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int MovieId { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
