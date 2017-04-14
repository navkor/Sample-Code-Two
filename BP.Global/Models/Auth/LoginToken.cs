using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BP.Auth
{
    public class LoginToken
    {
        public int ID { get; private set; }
        public string Token { get; set; }
        public DateTimeOffset TokenDate { get; set; }
        public virtual TokenProvider TokenProvider { get; set; }
        public virtual LoginAttribute LoginAttribute { get; set; }
    }
}