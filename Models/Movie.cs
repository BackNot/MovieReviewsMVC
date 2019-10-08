using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MovieReviews.Models
{
    public class Movie
    {
        [System.Web.Mvc.HiddenInput(DisplayValue = false)] // hide it
        [Required]
        public int MovieId { get; set; }
        [Required]
        public string MovieName { get; set; }
        [Required]
       
        public int MovieYear { get; set; }
        [Required]
        public string PathToImage { get; set; } // e.g. '~Contents/Images/the-matrix.png'
        [Required]
        public string Description { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public double Stars { get; set; } // stars from the admin 
        [Required]
        [DataType(DataType.MultilineText)]
        public string Review { get; set; } // review from the admin
        
        public virtual ICollection<UserReview> UserReviews { get; set; } 
        [Timestamp] // Concurrency handling
        public byte[] RowVersion { get; set; }
    }
}