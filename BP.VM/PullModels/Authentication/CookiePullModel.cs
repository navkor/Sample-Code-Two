using System;

namespace BP.VM.PullModels.Authentication
{
    public class CookiePullModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public DateTimeOffset Expiration { get; set; }
    }
}
