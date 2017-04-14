using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP
{
    public class MethodResults
    {
        public int ID { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }

    public class MethodResultsObject : MethodResults
    {
        public object ResultObject { get; set; }
    }
}
