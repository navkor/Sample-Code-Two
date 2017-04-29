using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BP.Web.Models
{
    public class UserModels
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public bool Me { get; set; }
        public bool EmailVerified { get; set; }
    }

    public class CreateUser
    {
        [Required(ErrorMessage = "Username is required, though you could just use the email address")]
        [Display(Name = "User name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required to create a user")]
        [Display(Name = "Password", Prompt = "Use a strong password with lower, upper, number, and character")]
        [DataType(DataType.Password)]
        public string PassWord { get; set; }
        [Required(ErrorMessage = "An email address is required")]
        [EmailAddress]
        [Display(Name = "Email Address", Prompt = "A standard and correctly formatted email address")]
        public string EmailAddress { get; set; }
        public string SelectedRole { get; set; }
        public IEnumerable<SelectListItem> Roles { get; set; }
    }

    public class EditUser : UserModels
    {
        public string SelectedRole { get; set; }
        public IEnumerable<SelectListItem> Roles { get; set; }
    }

    public class EditUserName
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Confirm UserName")]
        [System.ComponentModel.DataAnnotations.Compare("UserName", ErrorMessage = "Your username doesn't match the confirmation.  Please check the spelling and try again.")]
        public string ConfirmUserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }
    }
}