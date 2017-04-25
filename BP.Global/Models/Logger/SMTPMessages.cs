using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Global.Models.Logger
{
    public class SMTPMessage
    {
        public SMTPMessage()
        {
            SMTPAttachments = new HashSet<SMTPAttachment>();
            ToAddresses = new HashSet<SMTPTo>();
        }
        public int ID { get; private set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime SendDate { get; set; }
        public bool Sent { get; set; }
        public int Importance { get; set; }
        public virtual SMTPAccount SMTPAccount { get; set; }
        public virtual ICollection<SMTPTo> ToAddresses { get; set; }
        public virtual ICollection<SMTPAttachment> SMTPAttachments { get; set; }
    }

    public class SMTPTo
    {
        public SMTPTo()
        {
            SMTPMessages = new HashSet<SMTPMessage>();
        }
        public int ID { get; private set; }
        public string EmailAddress { get; set; }
        public string Name { get; set; }
        public virtual ICollection<SMTPMessage> SMTPMessages { get; set; }
    }

    public class SMTPAttachment
    {
        public int ID { get; private set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public virtual SMTPMessage SMTPMessages { get; set; }
    }
}
