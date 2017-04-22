using System.Collections.Generic;

namespace BP.Global.Models.Main
{
    public class BusinessType
    {
        public BusinessType()
        {
            BusinessAttributes = new HashSet<BusinessAttribute>();
        }
        public int ID { get; private set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public virtual ICollection<BusinessAttribute> BusinessAttributes { get; set; }
    }
}