using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPGMakerDecrypter.Decrypter.Exceptions;

namespace RPGMakerDecrypter.Decrypter
{
    /// <summary>
    /// Represents RGSSAD format used in RPG Maker VX Ace.
    /// </summary>
    public class RGSSADv3 : RGSSAD
    {
        public RGSSADv3(BinaryReader inBinaryReader) : base(inBinaryReader)
        {
        }

        protected override void PrepareKey() {
            key = (uint)BinaryReader.ReadInt32() * 9 + 3;
        }

        protected override void ReadRGSSAD()
        {
            ArchivedFiles = new List<ArchivedFile>();

            while (true)
            {
                ArchivedFile archivedFile = new ArchivedFile();
                archivedFile.Offset = DecryptInteger(BinaryReader.ReadInt32());
                archivedFile.Size = DecryptInteger(BinaryReader.ReadInt32());
                archivedFile.Key = (uint)DecryptInteger(BinaryReader.ReadInt32());

                int length = DecryptInteger(BinaryReader.ReadInt32());

                if (archivedFile.Offset == 0)
                {
                    break;
                }

                archivedFile.Name = DecryptFilename(BinaryReader.ReadBytes(length));
                ArchivedFiles.Add(archivedFile);
            }
        }

        
        protected override int DecryptInteger(int value)
        {
            long result = value ^ key;
            return (int)result;
        }

        protected override string DecryptFilename(byte[] encryptedName)
        {
            byte[] decryptedName = new byte[encryptedName.Length];

            byte[] keyBytes = BitConverter.GetBytes(key);
            for (int i = 0; i <= encryptedName.Length - 1; i++)
            {
                decryptedName[i] = (byte)(encryptedName[i] ^ keyBytes[i % 4]);
            }

            string result = Encoding.UTF8.GetString(decryptedName);

            return result;
        }
    }
}
