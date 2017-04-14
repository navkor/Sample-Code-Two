using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BP.Auth
{
    public class Title
    {
        public Title()
        {
            IdentityAttributes = new HashSet<IdentityAttribute>();
        }
        public int ID { get; private set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public virtual ICollection<IdentityAttribute> IdentityAttributes { get; set; }
    }
}
