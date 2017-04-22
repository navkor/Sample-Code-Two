using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BP.Global.Models.Main
{
    public class StreetAttribute
    {
        public StreetAttribute()
        {
            Streets = new HashSet<StringTable>();
        }
        [Key]
        [ForeignKey("Address")]
        public int AddressID { get; private set; }
        public virtual Address Address { get; set; }
        public virtual ICollection<StringTable> Streets { get; set; }
    }
}