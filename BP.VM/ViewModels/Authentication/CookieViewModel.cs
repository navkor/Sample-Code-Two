using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.VM.ViewModels.Authentication
{
    public class CookieLoginViewModel
    {
        public int userID { get; set; }
        public string Token { get; set; }
        public bool RememberMe { get; set; }
    }
}
