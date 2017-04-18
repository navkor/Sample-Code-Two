using System.Collections.Generic;

namespace BP.Global.Models.Main
{
    public class LoginIdType
    {
        public LoginIdType()
        {
            LoginIds = new HashSet<LoginId>();
        }
        public int ID { get; private set; }
        public string Name { get; set; }
        public int Index { get; set; }
        public virtual ICollection<LoginId> LoginIds { get; set; }
    }
}