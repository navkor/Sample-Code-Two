using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BP.Auth
{
    public class AccountAttribute
    {
        public AccountAttribute()
        {
            AccountDates = new HashSet<DateTable>();
        }
        [Key]
        [ForeignKey("Registration")]
        public int RegistrationID { get; private set; }
        public virtual Registration Registration { get; set; }
        public bool Active { get; set; }
        public virtual ICollection<DateTable> AccountDates { get; set; }
        public virtual RegistrationType RegistrationType { get; set; }
        public virtual Role UserRole { get; set; }
    }
}