using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Global.Delegates
{
    public class AuthenticationEventArgs : EventArgs
    {
        public AuthenticationEventArgs()
        {

        }
        public AuthenticationEventArgs(bool success, string username)
        {
            Success = success;
            UserName = username;
        }
        public string UserName { get; set; }
        public bool Success { get; set; }
    }
}
