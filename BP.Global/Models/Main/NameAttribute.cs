using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BP.Global.Models.Main
{
    public class NameAttribute
    {
        public NameAttribute()
        {
            UserNames = new HashSet<UserName>();
        }
        [Key]
        [ForeignKey("ProfileAttribute")]
        public int ProfileAttributeID { get; private set; }
        public virtual ProfileAttribute ProfileAttribute { get; set; }
        public virtual ICollection<UserName> UserNames { get; set; }
        public virtual NameFormat NameFormat { get; set; }
        public virtual Title Title { get; set; }
    }
}