using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BP.Auth
{
    public class NameType
    {
        public NameType()
        {
            UserNames = new HashSet<UserName>();
        }
        public int ID { get; private set; }
        public string Type { get; set; }
        public int Index { get; set; }
        public virtual ICollection<UserName> UserNames { get; set; }
    }
}