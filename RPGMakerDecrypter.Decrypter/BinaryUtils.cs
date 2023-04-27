using System.IO;
using System.Text;

namespace RPGMakerDecrypter.Decrypter
{
    public static class BinaryUtils
    {
        /// <summary>
        /// Reads string that may contain '\0' from given binary reader.
        /// Seeks to end of the string after reading.
        /// </summary>
        /// <param name="binaryReader">The binary reader.</param>
        /// <param name="maxLength">The maximum length of the string</param>
        /// <returns>Found string</returns>
        public static string ReadString(BinaryReader binaryReader, int maxLength)
        {
            long beginPosition = binaryReader.BaseStream.Position;
            if (maxLength > binaryReader.BaseStream.Length - binaryReader.BaseStream.Position) maxLength = (int)(binaryReader.BaseStream.Length - binaryReader.BaseStream.Position);
            string result = Encoding.ASCII.GetString(binaryReader.ReadBytes(maxLength));

            // Seeking to end position of the string
            binaryReader.BaseStream.Seek(beginPosition + maxLength, SeekOrigin.Begin);
            return result;
        }
    }
}
