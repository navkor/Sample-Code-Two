using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BP.Global.Models.Main
{
    public class LoginAttribute
    {
        public LoginAttribute()
        {
            LoginIds = new HashSet<LoginId>();
        }
        [Key]
        [ForeignKey("Account")]
        public int AccountID { get; private set; }
        public virtual Account Account { get; set; }
        public virtual ICollection<LoginId> LoginIds { get; set; }
    }
}