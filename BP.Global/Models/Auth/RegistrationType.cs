using System.Collections.Generic;

namespace BP.Auth
{
    public class RegistrationType
    {
        public RegistrationType()
        {
            AccountAttributes = new HashSet<AccountAttribute>();
        }
        public int ID { get; private set; }
        public string Name { get; set; }
        public int Index { get; set; }
        public virtual ICollection<AccountAttribute> AccountAttributes { get; set; }
    }
}