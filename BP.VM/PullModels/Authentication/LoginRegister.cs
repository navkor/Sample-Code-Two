using System.ComponentModel.DataAnnotations;
using BP.Global.Attributes.Authentication;
using BP.Auth;
using System.Web;

namespace BP.VM.PullModels.Authentication
{
    public class RegisterAccount
    {
        [Required(ErrorMessage = "Please include an email address")]
        [EmailAddress(ErrorMessage = "Please format your email address properly")]
        public string Email { get; set; }

        [Display(Name = "Confirm Email")]
        [Compare("Email", ErrorMessage = "Your email addresses didn't match")]
        public string ConfirmEmail { get; set; }

        [Display(Name = "Password")]
        [Password]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Your passwords don't match")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }

    public class ActivateEmailModel
    {
        public int ID { get; set; }
        public string ActivateString { get; set; }
    }

    public class LoginAccountStep1
    {
        [Required(ErrorMessage = "Please include an email address or user name")]
        [Display(Name = "Email/Username")]
        public string Email { get; set; }

    }

    public class LoginAccountStep2
    {
        public string Email { get; set; }
        [Display(Name = "Remember Me?", Description = "Saves a cookie to your computer and allows for easier login for 2 weeks.  Only use on personal computers.")]
        public bool RememberMe { get; set; }

        [Required(ErrorMessage = "Please enter a password before continuing")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class AccountLoggedIn
    {
        public Registration Registration { get; set; }
        public HttpCookie Cookie { get; set; }
        public string LoginToken { get; set; }
    }
}
