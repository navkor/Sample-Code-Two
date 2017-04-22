using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BP.Global.Models.Main
{
    public class BusinessProgramAttribute
    {
        [Key]
        [ForeignKey("BusinessAttribute")]
        public int BusinessAttributeID { get; private set; }
        public virtual BusinessAttribute BusinessAttribute { get; set; }
    }
}