using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BP.Global.Models.Main
{
    public class UserProgramAttribute
    {
        [Key]
        [ForeignKey("UserAttribute")]
        public int UserAttributeID { get; private set; }
        public virtual UserAttribute UserAttribute { get; set; }
    }
}