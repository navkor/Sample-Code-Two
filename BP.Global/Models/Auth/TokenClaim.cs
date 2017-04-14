using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BP.Auth
{
    public class TokenClaim
    {
        public TokenClaim()
        {
            ClaimDates = new HashSet<DateTable>();
        }
        public int ID { get; private set; }
        public string Claim { get; set; }
        public virtual ClaimEntity ClaimEntity { get; set; }
        public virtual LoginAttribute LoginAttribute { get; set; }
        public virtual ICollection<DateTable> ClaimDates { get; set; }
    }
}