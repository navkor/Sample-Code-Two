using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Global.Models.Main
{
    public class Account
    {
        public int ID { get; private set; }
        public virtual LoginAttribute LoginAttribute { get; set; }
    }
}
