using System.Collections.Generic;

namespace BP.Global.Models.Main
{
    public class Gender
    {
        public Gender()
        {
            ProfileAttributes = new HashSet<ProfileAttribute>();
        }
        public int ID { get; private set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public virtual ICollection<ProfileAttribute> ProfileAttributes { get; set; }
    }
}