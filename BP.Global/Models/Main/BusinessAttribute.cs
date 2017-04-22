using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BP.Global.Models.Main
{
    public class BusinessAttribute
    {
        public BusinessAttribute()
        {
            Addresses = new HashSet<Address>();
        }
        [Key]
        [ForeignKey("Account")]
        public int AccountID { get; private set; }
        public virtual Account Account { get; set; }
        public virtual BusinessType BusinessType { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
        public string BusinessName { get; set; }
    }
}