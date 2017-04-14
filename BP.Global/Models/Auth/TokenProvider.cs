using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BP.Auth
{
    public class TokenProvider
    {
        public TokenProvider()
        {
            LoginTokens = new HashSet<LoginToken>();
        }
        public int ID { get; private set; }
        public string Provider { get; set; }
        public int Index { get; set; }
        public virtual ICollection<LoginToken> LoginTokens { get; set; }
    }
}