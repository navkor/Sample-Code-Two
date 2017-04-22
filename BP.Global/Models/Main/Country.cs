using System.Collections.Generic;

namespace BP.Global.Models.Main
{
    public class Country
    {
        public Country()
        {
            Districts = new HashSet<District>();
        }
        public int ID { get; private set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public virtual ICollection<District> Districts { get; set; }
    }
}