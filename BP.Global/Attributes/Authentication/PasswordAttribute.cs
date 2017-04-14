using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BP.Global.Attributes.Authentication
{
    public class PasswordAttribute : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // find out if the password has the required amount of everthing
            var password = value.ToString();
            if (string.IsNullOrEmpty(password)) return new ValidationResult("A password is required.");
            if (password.Length < 8) return new ValidationResult("A proper password is at least 8 characters long");
            if (!IsGood(password)) return new ValidationResult("Please enter a password that contains: 1 Uppercase, 1 Lowercase, 1 number, and 1 special character of either * $ @ - _ , . ( ) ?");
            return ValidationResult.Success;
        }        

        private bool IsGood(string password)
        {
            var isGood = false;
            if (password.Any(x => IsUpperCase(x)))
            {
                if (password.Any(x => isLowerCase(x)))
                {
                    if (password.Any(x => char.IsDigit(x)))
                    {
                        if (password.Any(x => isSpecial(x)))
                        {
                            isGood = true;
                        }
                    }
                }
            }
            return isGood;
        }
        private bool IsUpperCase(char c)
        {
            var upperList = Enumerable.Range('A', 26).Select(x => (char)x);
            return upperList.Any(x => x.Equals(c));
        }
        private bool isLowerCase(char c)
        {
            var letterList = Enumerable.Range('a', 26).Select(x => (char)x);
            return letterList.Any(x => x.Equals(c));
        }
        private bool isNumber(char c)
        {
            var numberList = Enumerable.Range(0, 10).Select(x => x.ToString()).ToArray();
            return numberList.Any(x => x.Equals(c));
        }
        private bool isSpecial(char c)
        {
            var symbolList = new List<char> {
                '*','$','@','-','_','(',')','.',',','?'
            };
            return symbolList.Any(x => x.Equals(c));
        }
    }
}
