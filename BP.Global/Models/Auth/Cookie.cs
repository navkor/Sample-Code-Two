using System;

namespace BP.Auth
{
    public class Cookie
    {
        public int ID { get; private set; }
        public string Code { get; set; }
        public string Data { get; set; }
        public virtual CookieType CookieType { get; set; }
        public DateTimeOffset DateSet { get; set; }
        public DateTimeOffset Expiration { get; set; }
    }
}
