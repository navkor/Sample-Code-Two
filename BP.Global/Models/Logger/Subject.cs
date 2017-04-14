using System.Collections.Generic;

namespace BP.Global.Models.Logger
{
    public class Subject : ISubjectId
    {
        public Subject()
        {
            Logs = new HashSet<Log>();
        }
        public int ID { get; private set; }
        public string SubjectLine { get; set; }
        public virtual ICollection<Log> Logs { get; set; }
    }
}
