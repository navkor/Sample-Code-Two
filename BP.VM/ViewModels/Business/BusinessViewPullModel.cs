using System.Collections.Generic;

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

    }
}
