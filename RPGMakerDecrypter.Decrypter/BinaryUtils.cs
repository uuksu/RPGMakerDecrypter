using System.IO;
using System.Text;

namespace RPGMakerDecrypter.Decrypter
{
    public static class BinaryUtils
    {
        /// <summary>
        /// Reads C style string from given binary reader.
        /// Seeks to end of the string after reading.
        /// </summary>
        /// <param name="binaryReader">The binary reader.</param>
        /// <param name="maxLength">The maximum length of the string</param>
        /// <returns>Found string</returns>
        public static string ReadCString(BinaryReader binaryReader, int maxLength)
        {
            long beginPosition = binaryReader.BaseStream.Position;
            int stringLenght = 0;

            // Searching for end of the C string (byte == 0, NUL character)
            do
            {
                byte readByte = binaryReader.ReadByte();
                if (readByte == 0)
                    break;

                stringLenght += 1;
            } while (stringLenght < maxLength);

            // Seeking back to beginning
            binaryReader.BaseStream.Seek(beginPosition, SeekOrigin.Begin);

            string result = Encoding.ASCII.GetString(binaryReader.ReadBytes(stringLenght));

            // Seeking to end position of the string
            binaryReader.BaseStream.Seek(beginPosition + stringLenght + 1, SeekOrigin.Begin);

            return result;
        }
    }
}
