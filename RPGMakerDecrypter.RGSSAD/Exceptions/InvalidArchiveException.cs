using System;

namespace RPGMakerDecrypter.Decrypter.Exceptions
{
    public class InvalidArchiveException : Exception
    {
        public InvalidArchiveException(string message) : base(message)
        {
        }
    }
}
