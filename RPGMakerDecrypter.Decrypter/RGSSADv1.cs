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
    /// Represents RGSSAD format used in RPG Maker XP and VX.
    /// </summary>
    public class RGSSADv1 : RGSSAD
    {
        public RGSSADv1(BinaryReader inBinaryReader) : base(inBinaryReader)
        {
        }

        protected override void PrepareKey()
        {
            key = Constants.RGASSADv1Key;
        }

        protected override void ReadRGSSAD()
        {
            ArchivedFiles = new List<ArchivedFile>();
            while (true)
            {
                ArchivedFile archivedFile = new ArchivedFile();

                int length = DecryptInteger(BinaryReader.ReadInt32());
                archivedFile.Name = DecryptFilename(BinaryReader.ReadBytes(length));
                archivedFile.Size = DecryptInteger(BinaryReader.ReadInt32());
                archivedFile.Offset = BinaryReader.BaseStream.Position;
                archivedFile.Key = key;
                ArchivedFiles.Add(archivedFile);

                BinaryReader.BaseStream.Seek(archivedFile.Size, SeekOrigin.Current);
                if (BinaryReader.BaseStream.Position == BinaryReader.BaseStream.Length)
                    break;
            }
        }

        protected override int DecryptInteger(int value)
        {
            long result = value ^ key;
            ShiftKey();
            return (int)result;
        }

        private void ShiftKey()
        {
            key = (key * 7) + 3;
        }

        
        protected override string DecryptFilename(byte[] encryptedName)
        {
            byte[] decryptedName = new byte[encryptedName.Length];

            for (int i = 0; i <= encryptedName.Length - 1; i++)
            {
                decryptedName[i] = (byte)(encryptedName[i] ^ (key & 0xff));
                ShiftKey();
            }

            string result = Encoding.UTF8.GetString(decryptedName);
            return result;
        }
    }
}
