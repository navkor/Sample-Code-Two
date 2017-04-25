using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.VM.ViewModels.Core.SMTP
{
    public class SMTPCreateMessage
    {
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime SendDate { get; set; }
        public int Importance { get; set; }
        public IEnumerable<SMTPTOAddress> ToAddresses { get; set; }
        public IEnumerable<SMTPAttachment>  Attachments { get; set; }
    }

    public class SMTPTOAddress
    {
        public int ID { get; set; }
        public string EmailAddress { get; set; }
        public string Name { get; set; }
    }

    public class SMTPAttachment
    {
        public string Name { get; set; }
        public string Location { get; set; }
    }
}
