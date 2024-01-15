using System;
using System.IO;

namespace RPGMakerDecrypter.Common
{
    public static class ExceptionLogger
    {
        public static string LogException(Exception exception)
        {
            var outputFilePath = Path.Combine(Path.GetTempPath(), $"RPGMakerDecrypter-{Guid.NewGuid()}.log");
            File.WriteAllText(outputFilePath, exception.ToString());

            return outputFilePath;
        }
    }
}
