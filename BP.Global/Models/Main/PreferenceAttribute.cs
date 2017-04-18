using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BP.Global.Models.Main
{
    public class PreferenceAttribute
    {
        public PreferenceAttribute()
        {
            NewsLetters = new HashSet<NewsLetter>();
        }
        [Key]
        [ForeignKey("LoginId")]
        public int LoginIdID { get; private set; }
        public virtual LoginId LoginId { get; set; }
        public virtual ICollection<NewsLetter> NewsLetters { get; set; }
    }
}