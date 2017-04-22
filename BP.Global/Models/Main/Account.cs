using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Global.Models.Main
{
    public class Account
    {
        public Account()
        {
            PreferredEntities = new HashSet<EntityPreferenceAttribute>();
            AvoidEntities = new HashSet<EntityPreferenceAttribute>();
        }
        public int ID { get; private set; }
        public virtual LoginAttribute LoginAttribute { get; set; }
        public virtual EntityAttribute EntityAttribute { get; set; }
        public virtual BusinessAttribute BusinessAttribute { get; set; }
        public virtual UserAttribute UserAttribute { get; set; }
        public virtual ICollection<EntityPreferenceAttribute> PreferredEntities { get; set; }
        public virtual ICollection<EntityPreferenceAttribute> AvoidEntities { get; set; }
    }
}
