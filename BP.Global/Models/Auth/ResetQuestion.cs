using System;

namespace BP.Auth
{
    public class ResetQuestion
    {
        public int ID { get; private set; }
        public virtual IdentityAttribute IdentityAttribute { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}