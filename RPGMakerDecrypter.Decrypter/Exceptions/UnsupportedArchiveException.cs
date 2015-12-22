using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGMakerDecrypter.Decrypter.Exceptions
{
    public class UnsupportedArchiveException : Exception
    {
        public UnsupportedArchiveException(string message) : base(message)
        {
        }
    }
}
