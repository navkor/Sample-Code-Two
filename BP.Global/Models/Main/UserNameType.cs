using System.Collections.Generic;

namespace BP.Global.Models.Main
{
    public class UserNameType
    {
        public UserNameType()
        {
            UserNames = new HashSet<UserName>();
            NameFormatMaps = new HashSet<NameFormatMap>();
        }
        public int ID { get; private set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public virtual ICollection<UserName> UserNames { get; set; }
        public virtual ICollection<NameFormatMap> NameFormatMaps { get; set; }
    }
}