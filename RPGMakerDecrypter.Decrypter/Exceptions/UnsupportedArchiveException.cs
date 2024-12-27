using System;

namespace RPGMakerDecrypter.Decrypter.Exceptions
{
    public class UnsupportedArchiveException : Exception
    {
        public UnsupportedArchiveException(string message) : base(message)
        {
        }
    }
}
