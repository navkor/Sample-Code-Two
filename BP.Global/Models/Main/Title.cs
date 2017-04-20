using System.Collections.Generic;

namespace BP.Global.Models.Main
{
    public class Title
    {
        public Title()
        {
            NameAttributes = new HashSet<NameAttribute>();
        }
        public int ID { get; private set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public virtual ICollection<NameAttribute> NameAttributes { get; set; }
    }
}