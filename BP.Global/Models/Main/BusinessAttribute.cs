using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BP.Global.Models.Main
{
    public class BusinessAttribute
    {
        [Key]
        [ForeignKey("Account")]
        public int AccountID { get; private set; }
        public virtual Account Account { get; set; }
        public virtual BusinessType BusinessType { get; set; }
        public string BusinessName { get; set; }
    }
}