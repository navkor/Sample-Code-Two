using System.Collections.Generic;

namespace BP.Global.Models.Logger
{
    public class System : INameId
    {
        public System()
        {
            Logs = new HashSet<Log>();
        }
        public int ID { get; private set; }
        public string Name { get; set; }
        public virtual ICollection<Log> Logs { get; set; }
    }
}
