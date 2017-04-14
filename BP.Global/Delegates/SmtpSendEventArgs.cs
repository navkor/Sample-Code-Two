using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Global.Delegates
{
    public class SmtpSendEventArgs : EventArgs
    {
        public SmtpSendEventArgs()
        {

        }
        public SmtpSendEventArgs(bool success, string purpose)
        {
            Purpose = purpose;
            Success = success;
        }

        public string Purpose { get; set; }
        public bool Success { get; set; }
    }
}
