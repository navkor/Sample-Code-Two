using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BP.Auth
{
    public class Role
    {
        public Role()
        {
            AccountAttributes = new HashSet<AccountAttribute>();
        }
        public int ID { get; private set; }
        public virtual ICollection<AccountAttribute> AccountAttributes { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public virtual RoleGroup RoleGroup { get; set; }
    }
}