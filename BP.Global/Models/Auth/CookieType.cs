using System.Collections.Generic;

namespace BP.Auth
{
    public class CookieType
    {
        public int ID { get; private set; }
        public string Name { get; set; }
        public int Index { get; set; }
        public virtual ICollection<Cookie> Cookies { get; set; }
    }
}