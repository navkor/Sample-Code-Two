using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BP.Auth
{
    public class PhoneNumberType
    {
        public PhoneNumberType()
        {
            PhoneNumbers = new HashSet<PhoneNumber>();
        }
        public int ID { get; private set; }
        public string TypeName { get; set; }
        public int Index { get; set; }
        public virtual ICollection<PhoneNumber> PhoneNumbers { get; set; }
    }
}