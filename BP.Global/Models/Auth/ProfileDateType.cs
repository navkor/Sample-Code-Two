using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BP.Auth
{
    public class ProfileDateType
    {
        public ProfileDateType()
        {
            DateTables = new HashSet<DateTable>();
        }
        public int ID { get; private set; }
        public int Index { get; set; }
        public string TypeName { get; set; }
        public virtual ICollection<DateTable> DateTables { get; set; }
    }
}