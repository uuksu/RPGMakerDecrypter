using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RPGMakerDecrypter.MVMZ
{
    public abstract class DirectoryFilesDecryptor
    {
        private readonly FileDecryptor _fileDecryptor = new FileDecryptor();
        
        protected void Decrypt(byte[] key, string inputPath, Dictionary<string, string> fileExtensionMaps,
            bool deleteEncrypted, bool overwrite)
        {
            var directory = new DirectoryInfo(inputPath);

            var extensions = fileExtensionMaps.Keys.ToArray();
            
            var encryptedFiles = GetEncryptedFiles(directory, extensions);
            if (!encryptedFiles.Any())
            {
                return;
            }
            
            foreach (var encryptedFile in encryptedFiles)
            {
                // Encrypted files have a changed extensions, map back to the original extension and create the target path for new file
                var fileNameWithoutExtension = new string(encryptedFile.Name.Take(encryptedFile.Name.Length - encryptedFile.Extension.Length).ToArray());
                var realExtension = fileExtensionMaps[encryptedFile.Extension];
                var targetDirectory = encryptedFile.Directory.FullName;
                var targetFilePath = Path.Combine(targetDirectory, $"{fileNameWithoutExtension}{realExtension}");

                if (!overwrite && File.Exists(targetFilePath))
                {
                    continue;
                }
                
                var decryptedFile = _fileDecryptor.Decrypt(key, encryptedFile.FullName);
                
                File.WriteAllBytes(targetFilePath, decryptedFile);

                if (deleteEncrypted)
                {
                    File.Delete(encryptedFile.FullName);
                }
            }
        }

        private FileInfo[] GetEncryptedFiles(DirectoryInfo directory, string[] fileExtensions)
        {
            List<FileInfo> encryptedFiles = new List<FileInfo>();
            
            foreach (var fileExtension in fileExtensions)
            {
                encryptedFiles.AddRange(directory.GetFiles($"*{fileExtension}", SearchOption.AllDirectories));
            }

            return encryptedFiles.ToArray();
        }
    }
}