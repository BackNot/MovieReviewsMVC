using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using System.Web.Mvc;

namespace MovieReviews.Models.VM
{
    public class RegisterModel
    {
        [Required(ErrorMessageResourceName = "FirstNameRequired", ErrorMessageResourceType = typeof(Resources.Resource))]
        [Display(Name="FirstName",ResourceType = typeof(Resources.Resource))]
        public string FirstName { get; set; }
        [Required(ErrorMessageResourceName = "LastNameRequired", ErrorMessageResourceType = typeof(Resources.Resource))]
        [Display(Name = "LastName",ResourceType = typeof(Resources.Resource))]
        public string LastName { get; set; }
        [Required(ErrorMessageResourceName = "UserNameRequired", ErrorMessageResourceType = typeof(Resources.Resource))]
        [Display(Name="UserName",ResourceType = typeof(Resources.Resource))]
        [Remote("IsUserNameValid","Auth")]
        public string UserName { get; set; }
        [Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(Resources.Resource))]
        [Display(Name="Email",ResourceType = typeof(Resources.Resource))]
        [Remote("IsEmailValid","Auth")]
        public string Email { get; set; }
        [Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(Resources.Resource))]
        [Display(Name="Password",ResourceType = typeof(Resources.Resource))]
        [MinLength(6,ErrorMessage="Password must be over 6 characters")]
        public string Password { get; set; }

        [Required(ErrorMessageResourceName = "RePassword", ErrorMessageResourceType = typeof(Resources.Resource))]
        [Display(Name = "RePassword", ResourceType = typeof(Resources.Resource))]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessageResourceName = "PasswordDontMatch", ErrorMessageResourceType = typeof(Resources.Resource))]
        public string RePassword { get; set; }
    }
}