using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Display(Name = "Business Name")]
        [Required(ErrorMessage = "Please include a name for this business")]
        public string BusinessName { get; set; }
        [Display(Name = "Business Type")]
        public int BusinessTypeId { get; set; }
        public IEnumerable<SelectListItem> BusinessTypes { get; set; }
    }
}
