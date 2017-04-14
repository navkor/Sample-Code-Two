using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BP.Auth
{
    public class EmailAddress
    {
        public EmailAddress()
        {
            EmailDates = new HashSet<DateTable>();
        }
        public int ID { get; private set; }
        public string Email { get; set; }
        public bool Default { get; set; }
        public bool Validated { get; set; }
        public string ValidationString { get; set; }
        public virtual ProfileAttribute ProfileAttribute { get; set; }
        public virtual ICollection<DateTable> EmailDates { get; set; }
    }
}