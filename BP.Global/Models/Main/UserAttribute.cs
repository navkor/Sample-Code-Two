using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BP.Global.Models.Main
{
    public class UserAttribute
    {
        [Key]
        [ForeignKey("Account")]
        public int AccountID { get; private set; }
        public virtual Account Account { get; set; }
        public virtual UserProgramAttribute UserProgramAttribute { get; set; }
    }
}