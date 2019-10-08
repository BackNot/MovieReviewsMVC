using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace MovieReviews.Models.VM
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessageResourceName = "OldPasswordRequired", ErrorMessageResourceType = typeof(Resources.Resource))]
        [Display(Name = "OldPassword", ResourceType = typeof(Resources.Resource))]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required(ErrorMessageResourceName = "ReNewPasswordRequired", ErrorMessageResourceType = typeof(Resources.Resource))]
        [Display(Name = "NewPassword", ResourceType = typeof(Resources.Resource))]
        [MinLength(6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessageResourceName = "ReNewPasswordRequired", ErrorMessageResourceType = typeof(Resources.Resource))]
        [Display(Name = "ReNewPassword", ResourceType = typeof(Resources.Resource))]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessageResourceName = "PasswordDontMatch", ErrorMessageResourceType = typeof(Resources.Resource))]
        public string ReNewPassword { get; set; }
    }
}