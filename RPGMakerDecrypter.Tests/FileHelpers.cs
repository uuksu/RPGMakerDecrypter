using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPGMakerDecrypter.Decrypter;

namespace RPGMakerDecrypter.Tests
{
    static class FileHelpers
    {
        public static string TempDirectoryPath = "Temp";

        public static void CopyArchives()
        {
            if (Directory.Exists(TempDirectoryPath))
            {
                Directory.Delete(TempDirectoryPath, true);
            }

            Directory.CreateDirectory(TempDirectoryPath);

            File.Copy(Path.Combine("../../EncryptedArchives", Constants.RpgMakerXpArchiveName), Path.Combine(TempDirectoryPath, Constants.RpgMakerXpArchiveName));
            File.Copy(Path.Combine("../../EncryptedArchives", Constants.RpgMakerVxArchiveName), Path.Combine(TempDirectoryPath, Constants.RpgMakerVxArchiveName));
            File.Copy(Path.Combine("../../EncryptedArchives", Constants.RpgMakerVxAceArchiveName), Path.Combine(TempDirectoryPath, Constants.RpgMakerVxAceArchiveName));
        }

        public static void Cleanup()
        {
            File.Delete(Path.Combine(TempDirectoryPath, Constants.RpgMakerXpArchiveName));
            File.Delete(Path.Combine(TempDirectoryPath, Constants.RpgMakerVxArchiveName));
            File.Delete(Path.Combine(TempDirectoryPath, Constants.RpgMakerVxAceArchiveName));

            Directory.Delete(TempDirectoryPath);
        }
    }
}
