using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BP.Auth
{
    public class ProfileAttribute
    {
        public ProfileAttribute()
        {
            EmailAddresses = new HashSet<EmailAddress>();
            PhoneNumbers = new HashSet<PhoneNumber>();
        }
        [Key]
        [ForeignKey("Registration")]
        public int RegistrationID { get; private set; }
        public virtual Registration Registration { get; set; }
        public string Biography { get; set; }
        public virtual ICollection<EmailAddress> EmailAddresses { get; set; }
        public virtual ICollection<PhoneNumber> PhoneNumbers { get; set; }
    }
}