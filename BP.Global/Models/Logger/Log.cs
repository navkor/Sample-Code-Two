using System;

namespace BP.Global.Models.Logger
{
    public class Log
    {
        public int ID { get; private set; }
        public string Body { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual Instigator Instigator { get; set; }
        public virtual System System { get; set; }
        public DateTimeOffset DateLine { get; set; }
    }
}
