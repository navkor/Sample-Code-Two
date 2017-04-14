using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.VM.PullModels.Authentication
{
    public class ResetPullModel
    {
        [Required(ErrorMessage = "Please include an email before continuing")]
        [EmailAddress(ErrorMessage = "Your email is not formatted properly")]
        public string EmailAddress { get; set; }
    }
}
