using System;

namespace RPGMakerDecrypter.RGSSAD.Exceptions
{
    public class InvalidArchiveException : Exception
    {
        public InvalidArchiveException(string message) : base(message)
        {
        }
    }
}
