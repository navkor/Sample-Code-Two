using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Global.Models.Logger
{
    public class SMTPAccount
    {
        public SMTPAccount()
        {
            SendDates = new HashSet<SMTPSendDate>();
            SMTPMessages = new HashSet<SMTPMessage>();
        }
        public int ID { get; private set; }
        public string UserName { get; set; }
        public string AppSettings { get; set; }
        public int DailyLimits { get; set; }
        public int AddressLimits { get; set; }
        public int PerMessageAddressLimits { get; set; }
        public virtual ICollection<SMTPMessage> SMTPMessages { get; set; }
        public virtual ICollection<SMTPSendDate> SendDates { get; set; }
    }

    public class SMTPSendDate
    {
        public int ID { get; private set; }
        public DateTime SendDate { get; set; }
        public int SendCount { get; set; }
        public int AddressCount { get; set; }
        public virtual SMTPAccount SMTPAccounts { get; set; }
    }
}
