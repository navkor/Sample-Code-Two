using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BP.Auth
{
    public class LoginAttribute
    {
        public LoginAttribute()
        {
            LoginDates = new HashSet<DateTable>();
            LoginTokens = new HashSet<LoginToken>();
            TokenClaims = new HashSet<TokenClaim>();
        }
        [Key]
        [ForeignKey("Registration")]
        public int RegistrationID { get; private set; }
        public virtual Registration Registration { get; set; }
        public virtual ICollection<DateTable> LoginDates { get; set; }
        public virtual ICollection<LoginToken> LoginTokens { get; set; }
        public virtual ICollection<TokenClaim> TokenClaims { get; set; }
    }
}