using System.Collections.Generic;

namespace BP.Auth
{
    public class ClaimEntity
    {
        public ClaimEntity()
        {
            TokenClaims = new HashSet<TokenClaim>();
        }
        public int ID { get; private set; }
        public string ClaimName { get; set; }
        public int Index { get; set; }
        public virtual ICollection<TokenClaim> TokenClaims { get; set; }
    }
}