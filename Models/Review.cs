using MovieReviews.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MovieReviews.Models
{
    public class UserReview
    {
        [System.Web.Mvc.HiddenInput(DisplayValue = false)] // hide it
        [Key]
        public int Id { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string Review { get; set; }
        public Movie Movie { get; set; }
        public virtual ApplicationUser User { get; set; }
        [Timestamp] // Concurrency handling
        public byte[] RowVersion { get; set; }
    }
}