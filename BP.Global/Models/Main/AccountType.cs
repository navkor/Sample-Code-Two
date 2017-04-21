using System.Collections.Generic;

namespace BP.Global.Models.Main
{
    public class AccountType
    {
        public AccountType()
        {
            EntityAttributes = new HashSet<EntityAttribute>();
        }
        public int ID { get; private set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public virtual ICollection<EntityAttribute> EntityAttributes { get; set; }
    }
}