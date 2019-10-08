using MovieReviews.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MovieReviews.Models.VM
{
    public class AddReviewModel
    {
        [Required]
        [DataType(DataType.MultilineText)]
        public string Review { get; set; }

        public string MovieName { get; set; }
       
    }
}