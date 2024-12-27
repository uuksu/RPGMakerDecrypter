using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using RPGMakerDecrypter.MVMZ.Exceptions;

namespace RPGMakerDecrypter.MVMZ
{
    public class EncryptionKeyFinder
    {
        public byte[] FindKey(string inputPath)
        {
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

            var md5Hash = (string)hashValue;
                
            if (string.IsNullOrWhiteSpace(md5Hash) || md5Hash.Length != 32)
            {
                throw new EncryptionKeyException("Found encryption key but it is too short.");
            }
                
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