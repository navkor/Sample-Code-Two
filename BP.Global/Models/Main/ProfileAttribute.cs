using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BP.Global.Models.Main
{
    public class ProfileAttribute
    {
        [Key]
        [ForeignKey("LoginId")]
        public int LoginIdID { get; private set; }
        public virtual LoginId LoginId { get; set; }
        public string Bio { get; set; }
        public DateTime BirthDate { get; set; }
        public virtual Gender Gender { get; set; }
        public string Hobbies { get; set; }
        public string Goals { get; set; }
    }
}