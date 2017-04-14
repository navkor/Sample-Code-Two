using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP
{
    public interface INameId
    {
        int ID { get; }
        string Name { get; set; }
    }

    public interface ISubjectId
    {
        int ID { get; }
        string SubjectLine { get; set; }
    }
}
