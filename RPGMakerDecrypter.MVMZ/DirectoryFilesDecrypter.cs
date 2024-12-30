using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RPGMakerDecrypter.MVMZ
{
    public abstract class DirectoryFilesDecrypter
    {
        private readonly FileDecrypter _fileDecrypter = new FileDecrypter();
        
        /// <summary>
        /// Decrypts files in a directory based on the provided key and file extension mappings.
        /// </summary>
        /// <param name="key">The encryption key.</param>
        /// <param name="inputPath">The path to the directory containing the encrypted files.</param>
        /// <param name="fileExtensionMaps">A dictionary mapping encrypted file extensions to their original extensions.</param>
        /// <param name="deleteEncrypted">Whether to delete the original encrypted files after decryption.</param>
        /// <param name="overwrite">Whether to overwrite existing decrypted files.</param>
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
                
                var decryptedFile = _fileDecrypter.Decrypt(key, encryptedFile.FullName);
                
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