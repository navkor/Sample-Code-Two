using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.VM.ViewModels.AccountAdmin
{
    public class AccountAdminViewModel
    {
        public int ID { get; set; }
        public string AccountName { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string AccountType { get; set; }
        public DateTimeOffset Created { get; set; }
    }
}
