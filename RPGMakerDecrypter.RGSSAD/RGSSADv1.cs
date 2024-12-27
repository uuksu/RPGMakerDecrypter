using System.Collections.Generic;
using System.IO;
using System.Text;
using RPGMakerDecrypter.RGSSAD.Exceptions;

namespace RPGMakerDecrypter.RGSSAD
{
    /// <summary>
    /// Represents RGSSAD format used in RPG Maker XP and VX.
    /// </summary>
    public class RGSSADv1 : RGSSAD
    {
        public RGSSADv1(string filePath) : base(filePath)
        {
            var version = GetVersion();

            if (version != Constants.RGASSDv1)
            {
                throw new InvalidArchiveException("Archive is in invalid format.");
            }

            ReadRGSSAD();
        }

        /// <summary>
        /// Reads the contents of RGSSAD archive and populates ArchivedFiles property.
        /// </summary>
        private void ReadRGSSAD()
        {
            var key = Constants.RGASSADv1Key;

            ArchivedFiles = new List<ArchivedFile>();

            BinaryReader.BaseStream.Seek(8, SeekOrigin.Begin);
            while (true)
            {
                var archivedFile = new ArchivedFile();

                var length = DecryptInteger(BinaryReader.ReadInt32(), ref key);
                archivedFile.Name = DecryptFilename(BinaryReader.ReadBytes(length), ref key);
                archivedFile.Size = DecryptInteger(BinaryReader.ReadInt32(), ref key);
                archivedFile.Offset = BinaryReader.BaseStream.Position;
                archivedFile.Key = key;
                ArchivedFiles.Add(archivedFile);

                BinaryReader.BaseStream.Seek(archivedFile.Size, SeekOrigin.Current);
                if (BinaryReader.BaseStream.Position == BinaryReader.BaseStream.Length)
                    break;
            }
        }
        /// <summary>
        /// Decrypts integer from given value.
        /// Proceeds key forward by calculating new value.
        /// </summary>
        /// <param name="value">Encrypted value</param>
        /// <param name="key">Key</param>
        /// <returns>Decrypted integer</returns>
        private int DecryptInteger(int value, ref uint key)
        {
            var result = value ^ key;

            key *= 7;
            key += 3;

            return (int)result;
        }

        /// <summary>
        /// Decrypts file name from given bytes using given key.
        /// Proceeds key forward by calculating new value.
        /// </summary>
        /// <param name="encryptedName">Encrypted filename</param>
        /// <param name="key">Key</param>
        /// <returns>Decrypted filename</returns>
        private string DecryptFilename(byte[] encryptedName, ref uint key)
        {
            var decryptedName = new byte[encryptedName.Length];

            for (var i = 0; i <= encryptedName.Length - 1; i++)
            {
                decryptedName[i] = (byte)(encryptedName[i] ^ (key & 0xff));

                key *= 7;
                key += 3;
            }

            var result = Encoding.UTF8.GetString(decryptedName);

            return result;
        }
    }
}
