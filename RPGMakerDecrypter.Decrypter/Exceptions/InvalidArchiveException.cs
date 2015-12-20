using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGMakerDecrypter.Decrypter.Exceptions
{
    public class InvalidArchiveException : Exception
    {
        public InvalidArchiveException(string message) : base(message)
        {
        }
    }
}
