using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RPGMakerDecrypter.MVMZ.Exceptions;

namespace RPGMakerDecrypter.MVMZ
{
    public class EncryptionKeyFinder
    {
        /// <summary>
        /// Finds the encryption key from the System.json file in the given input path.
        /// </summary>
        /// <param name="inputPath">The path to search for the System.json file.</param>
        /// <returns>The encryption key as a byte array.</returns>
        /// <exception cref="EncryptionKeyException">Thrown if the System.json file is not found, or if it does not contain a valid encryption key.</exception>
        public byte[] FindKey(string inputPath)
        {
            // System.json file contains the encryption key
            var systemFile = Directory.GetFiles(inputPath, "System.json", SearchOption.AllDirectories).FirstOrDefault();

            if (systemFile == null || !File.Exists(systemFile))
            {
                throw new EncryptionKeyException("Unable to find the System.json file from the input path.");
            }
            
            var systemFileJson = File.ReadAllText(systemFile);
            var systemObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(systemFileJson);

            if (!systemObject.TryGetValue("encryptionKey", out var hashValue))
            {
                throw new EncryptionKeyException(
                    "System.json file does not contain encryption key, unable to decrypt files. " +
                    "It's also possible that the files are not encrypted.");
            }

            // Encryption key is a MD5 hash of the key given when deploying the game.
            var md5Hash = (string)hashValue;
                
            if (string.IsNullOrWhiteSpace(md5Hash) || md5Hash.Length != 32)
            {
                throw new EncryptionKeyException("Found encryption key but it is too short.");
            }
            
            // MD5 hash to be split into 16 bytes to get the actual encryption key
            return MD5HashToByteArray(md5Hash);

        }
        
        private byte[] MD5HashToByteArray(string md5HashString)
        {
            var byteArray = new byte[16];
            
            for (var i = 0; i < 16; i++)
            {
                byteArray[i] = Convert.ToByte(md5HashString.Substring(i * 2, 2), 16);
            }
            
            return byteArray;
        }
    }
}