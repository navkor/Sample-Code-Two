using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BP.Global.Models.Main
{
    public class EntityPreferenceAttribute
    {
        public EntityPreferenceAttribute()
        {
            AvoidAccounts = new HashSet<Account>();
            AvoidKeywords = new HashSet<KeyWord>();
            PreferredAccounts = new HashSet<Account>();
            PreferredKeywords = new HashSet<KeyWord>();
        }
        [Key]
        [ForeignKey("EntityAttribute")]
        public int EntityAttributeID { get; private set; }
        public virtual EntityAttribute EntityAttribute { get; set; }
        public virtual ICollection<KeyWord> AvoidKeywords { get; set; }
        public virtual ICollection<KeyWord> PreferredKeywords { get; set; }
        public virtual ICollection<Account> AvoidAccounts { get; set; }
        public virtual ICollection<Account> PreferredAccounts { get; set; }
    }
}