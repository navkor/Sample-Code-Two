using System.Security.Cryptography;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace BP
{
    public class EncryptionHandler : IDisposable
    {
        // Flag: Has Dispose already been called?
        bool disposed = false;
        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        public string SetPassword(string passWord, string salt)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes($"{salt}{passWord}"));

                foreach (Byte b in result)
                    Sb.Append(b.ToString());
            }

            return Sb.ToString();
        }

        public bool AreEqual(string password1, string password2)
        {
            return password1.Equals(password2);
        }

        private static Random random = new Random();
        public string RandomString(int length)
        {
            var letterList = Enumerable.Range('a', 26).Select(x => (char)x);
            var upperList = Enumerable.Range('A', 26).Select(x => (char)x);
            var numberList = Enumerable.Range(0, 10).Select(x => x.ToString()).ToArray();
            var totalList = new List<char>();
            totalList.AddRange(letterList);
            totalList.AddRange(upperList);
            totalList.AddRange(numberList.Select(x => Convert.ToChar(x)));
            return new string(Enumerable.Repeat(totalList, length)
              .Select(s => s[random.Next(s.Count)]).ToArray());
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;
            if (disposing)
            {
                handle.Dispose();
            }

            disposed = true;
        }
    }
}
