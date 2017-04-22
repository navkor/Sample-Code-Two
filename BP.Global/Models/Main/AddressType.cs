using System.Collections.Generic;

namespace BP.Global.Models.Main
{
    public class AddressType
    {
        public AddressType()
        {
            Addresses = new HashSet<Address>();
        }
        public int ID { get; private set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
    }
}