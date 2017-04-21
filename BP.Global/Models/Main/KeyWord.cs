using System.Collections.Generic;

namespace BP.Global.Models.Main
{
    public class KeyWord
    {
        public KeyWord()
        {
            PreferredEntities = new HashSet<EntityPreferenceAttribute>();
            AvoidEntities = new HashSet<EntityPreferenceAttribute>();
        }
        public int ID { get; private set; }
        public string Name { get; set; }
        public virtual ICollection<EntityPreferenceAttribute> PreferredEntities { get; set; }
        public virtual ICollection<EntityPreferenceAttribute> AvoidEntities { get; set; }
    }
}