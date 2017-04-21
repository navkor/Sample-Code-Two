using System.Collections.Generic;

namespace BP.Global.Models.Main
{
    public class AccountDateType
    {
        public AccountDateType()
        {
            AccountDates = new HashSet<AccountDate>();
        }
        public int ID { get; private set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public virtual ICollection<AccountDate> AccountDates { get; set; }
    }
}