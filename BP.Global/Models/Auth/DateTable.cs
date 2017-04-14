using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BP.Auth
{
    public class DateTable
    {
        public int ID { get; private set; }
        public DateTimeOffset DateLine { get; set; }
        public virtual ProfileDateType DateType { get; set; }
        public virtual IdentityAttribute IdentityAttribute { get; set; }
        public virtual EmailAddress EmailAddress { get; set; }
        public virtual TokenClaim TokenClaim { get; set; }
    }
}