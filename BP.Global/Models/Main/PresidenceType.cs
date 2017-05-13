using System.Collections.Generic;

namespace BP.Global.Models.Main
{
    public class PresidenceType
    {
        public PresidenceType()
        {
            LoginIds = new HashSet<LoginId>();
        }
        public int ID { get; private set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public ICollection<LoginId> LoginIds { get; set; }
    }
}