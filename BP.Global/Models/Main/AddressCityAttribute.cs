using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BP.Global.Models.Main
{
    public class AddressCityAttribute
    {
        [Key]
        [ForeignKey("Address")]
        public int AddressID { get; private set; }
        public virtual Address Address { get; set; }
        public virtual City City { get; set; }
    }
}