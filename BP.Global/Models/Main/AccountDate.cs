using System;

namespace BP.Global.Models.Main
{
    public class AccountDate
    {
        public int ID { get; private set; }
        public DateTimeOffset DateLine { get; set; }
        public virtual AccountDateType AccountDateType { get; set; }
        public virtual EntityAttribute EntityAttribute { get; set; }
    }
}