using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPGMakerDecrypter.RGSSAD;

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

            File.Copy(Path.Combine("EncryptedArchives", Constants.RpgMakerXpArchiveName), Path.Combine(TempDirectoryPath, Constants.RpgMakerXpArchiveName));
            File.Copy(Path.Combine("EncryptedArchives", Constants.RpgMakerVxArchiveName), Path.Combine(TempDirectoryPath, Constants.RpgMakerVxArchiveName));
            File.Copy(Path.Combine("EncryptedArchives", Constants.RpgMakerVxAceArchiveName), Path.Combine(TempDirectoryPath, Constants.RpgMakerVxAceArchiveName));
        }

        public static void CopyEncryptedFiles()
        {
            if (Directory.Exists(TempDirectoryPath))
            {
                Directory.Delete(TempDirectoryPath, true);
            }

            Directory.CreateDirectory(TempDirectoryPath);
            
            File.Copy(Path.Combine("EncryptedFiles", "Image"), Path.Combine(TempDirectoryPath, "Image"));
            File.Copy(Path.Combine("EncryptedFiles", "AudioOrbis"), Path.Combine(TempDirectoryPath, "AudioOrbis"));
            File.Copy(Path.Combine("EncryptedFiles", "AudioMpeg"), Path.Combine(TempDirectoryPath, "AudioMpeg"));
        }

        public static void CleanupArchives()
        {
            File.Delete(Path.Combine(TempDirectoryPath, Constants.RpgMakerXpArchiveName));
            File.Delete(Path.Combine(TempDirectoryPath, Constants.RpgMakerVxArchiveName));
            File.Delete(Path.Combine(TempDirectoryPath, Constants.RpgMakerVxAceArchiveName));

            Directory.Delete(TempDirectoryPath);
        }
        
        public static void CleanupEncryptedFiles()
        {
            File.Delete(Path.Combine(TempDirectoryPath, "Image"));
            File.Delete(Path.Combine(TempDirectoryPath, "AudioOrbis"));
            File.Delete(Path.Combine(TempDirectoryPath, "AudioMpeg"));

            Directory.Delete(TempDirectoryPath);
        }
    }
}
