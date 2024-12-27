using System;

namespace RPGMakerDecrypter.MVMZ.Exceptions
{
    public class EncryptionKeyException : Exception
    {
        public EncryptionKeyException(string message) : base(message)
        {
            
        }
    }
}