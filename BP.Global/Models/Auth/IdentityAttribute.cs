using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BP.Auth
{
    public class IdentityAttribute
    {
        public IdentityAttribute()
        {
            UserNames = new HashSet<UserName>();
            IdentityDates = new HashSet<DateTable>();
            ResetQuestions = new HashSet<ResetQuestion>();
        }
        [Key]
        [ForeignKey("Registration")]
        public int RegistrationID { get; private set; }
        public virtual Registration Registration { get; set; }
        public virtual Title Title { get; set; }
        public virtual ICollection<ResetQuestion> ResetQuestions { get; set; }
        public virtual ICollection<UserName> UserNames { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public virtual ICollection<DateTable> IdentityDates { get; set; }
    }
}