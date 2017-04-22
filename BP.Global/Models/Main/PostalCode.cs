using System.Collections.Generic;

namespace BP.Global.Models.Main
{
    public class PostalCode
    {
        public PostalCode()
        {
            Cities = new HashSet<City>();
        }
        public int ID { get; private set; }
        public string CodeLine { get; set; }
        public virtual FormName PostalCodeFormName { get; set; }
        public virtual ICollection<City> Cities { get; set; }
    }
}