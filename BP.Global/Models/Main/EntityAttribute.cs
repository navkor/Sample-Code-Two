using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BP.Global.Models.Main
{
    public class EntityAttribute
    {
        public EntityAttribute()
        {
            AccountDates = new HashSet<AccountDate>();
        }
        [Key]
        [ForeignKey("Account")]
        public int AccountID { get; private set; }
        public virtual Account Account { get; set; }
        public virtual AccountType AccountType { get; set; }
        public virtual ICollection<AccountDate> AccountDates { get; set; }
        public virtual EntityPreferenceAttribute EntityPreferenceAttribute { get; set; }
        public string AccountName { get; set; }
    }
}