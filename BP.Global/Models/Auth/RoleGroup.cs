using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BP.Auth
{
    public class RoleGroup
    {
        public RoleGroup()
        {
            Roles = new HashSet<Role>();
        }
        public int ID { get; private set; }
        public string Name { get; set; }
        public int Index { get; set; }
        public ICollection<Role> Roles { get; set; }
    }
}