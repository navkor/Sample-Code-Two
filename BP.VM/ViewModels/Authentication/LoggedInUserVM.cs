using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.VM.ViewModels.Authentication
{
    public class LoggedInUserVM
    {
        public string UserID { get; set; }
        public string UserRole { get; set; }
        public int RoleIndex { get; set; }
        public int RoleGroupIndex { get; set; }
        public bool HasPassword { get; set; }
    }
}
