using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BP.Auth
{
    public class PhoneNumber
    {
        public int ID { get; private set; }
        public string Number { get; set; }
        public string Extension { get; set; }
        public virtual PhoneNumberType PhoneNumberType { get; set; }
        public virtual ProfileAttribute ProfileAttribute { get; set; }
    }
}