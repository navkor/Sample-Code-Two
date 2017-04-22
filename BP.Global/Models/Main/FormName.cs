using System.Collections.Generic;

namespace BP.Global.Models.Main
{
    public class FormName
    {
        public FormName()
        {
            PostalCodes = new HashSet<PostalCode>();
            Districts = new HashSet<District>();
        }
        public int ID { get; private set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public virtual ICollection<PostalCode> PostalCodes { get; set; }
        public virtual ICollection<District> Districts { get; set; }
    }
}