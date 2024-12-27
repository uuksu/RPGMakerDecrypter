using System;

namespace RPGMakerDecrypter.RGSSAD.Exceptions
{
    public class UnsupportedArchiveException : Exception
    {
        public UnsupportedArchiveException(string message) : base(message)
        {
        }
    }
}
