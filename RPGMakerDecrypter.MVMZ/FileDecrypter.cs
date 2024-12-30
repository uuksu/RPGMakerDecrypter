using System.IO;
using System.Linq;

namespace RPGMakerDecrypter.MVMZ
{
    public class FileDecrypter
    {
        /// <summary>
        /// Decrypts a encrypted RPG Maker file using the provided key and input path.
        /// </summary>
        /// <param name="key">The encryption key.</param>
        /// <param name="inputPath">The path to the file to be decrypted.</param>
        /// <returns>The decrypted file data as a byte array.</returns>
        public byte[] Decrypt(byte[] key, string inputPath)
        {
            // Skip first 16 bytes from beginning of file (aka. fake header)
            var output = File.ReadAllBytes(inputPath).Skip(16).ToArray();
            
            // XOR back the first 16 bytes of the file that are encrypted with the key to get the decrypted bytes
            for (var i = 0; i < 16; i++)
            {
                output[i] = (byte) (output[i] ^ key[i]);
            }

            return output;
        }

    }
}