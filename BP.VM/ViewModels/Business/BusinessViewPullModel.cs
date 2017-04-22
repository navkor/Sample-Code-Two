using System.Collections.Generic;
using System.Web.Mvc;

namespace BP.VM.ViewModels.Business
{
    public class BusinessViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public IEnumerable<NameIdValueNoteLists> Logins { get; set; }
        public NameIdLists BusinessType { get; set; }
    }

    public class BusinessPullModel
    {
        public string BusinessName { get; set; }
        public int BusinessTypeId { get; set; }
        public IEnumerable<SelectListItem> BusinessTypes { get; set; }
    }
}
