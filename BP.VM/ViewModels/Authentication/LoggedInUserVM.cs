using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.VM.ViewModels.Authentication
{
    public class LoggedInUserVM
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string UserRole { get; set; }
        public int RoleIndex { get; set; }
    }
}
