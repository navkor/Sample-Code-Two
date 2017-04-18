using System.Collections.Generic;

namespace BP.Global.Models.Main
{
    public class NewsLetter
    {
        public NewsLetter()
        {
            PreferenceAttributes = new HashSet<PreferenceAttribute>();
        }
        public int ID { get; private set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public virtual ICollection<PreferenceAttribute> PreferenceAttributes { get; set; }
    }
}