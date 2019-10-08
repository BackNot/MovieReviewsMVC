using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MovieReviews.Models
{
    public class LogInModel
    {
        [Required(ErrorMessageResourceName = "UserName", ErrorMessageResourceType = typeof(Resources.Resource))]
        [Display(Name = "UserName", ResourceType = typeof(Resources.Resource))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "Password", ErrorMessageResourceType = typeof(Resources.Resource))]
        [Display(Name = "Password", ResourceType = typeof(Resources.Resource))]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "RememberMe", ResourceType = typeof(Resources.Resource))]
        public bool RememberMe { get; set; }

        [HiddenInput]
        public string ReturnUrl { get; set; }
    }
}