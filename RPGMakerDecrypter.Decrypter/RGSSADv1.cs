using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPGMakerDecrypter.Decrypter.Exceptions;

namespace RPGMakerDecrypter.Decrypter
{
    public class RGSSADv1 : RGSSAD
    {
        public RGSSADv1(string filePath) : base(filePath)
        {
            int version = GetVersion();

            if (version != Constants.RGASSDv1)
            {
                throw new InvalidArchiveException("Archive is in invalid format.");
            }

            ReadRGSSAD();
        }

        /// <summary>
        /// Extracts all files.
        /// </summary>
        /// <param name="outputDirectoryPath">Output directory path</param>
        /// <param name="overrideExisting">if set to true, overrides existing files</param>
        public void ExtractAllFiles(string outputDirectoryPath, bool overrideExisting = false)
        {
            foreach (ArchivedFile archivedFile in ArchivedFiles)
            {
                ExtractFile(archivedFile, outputDirectoryPath, overrideExisting);
            }
        }

        /// <summary>
        /// Extracts single file from the file.
        /// </summary>
        /// <param name="archivedFile">Archived file</param>
        /// <param name="outputDirectoryPath">Output directory path</param>
        /// <param name="overrideExisting">if set to true, overrides existing files</param>
        /// <exception cref="System.Exception">Invalid file path. Archive could be corrupted.</exception>
        public void ExtractFile(ArchivedFile archivedFile, string outputDirectoryPath, bool overrideExisting = false)
        {
            // Create output directory if it does not exist
            string directoryPath = Path.GetDirectoryName(archivedFile.Name);

            if (directoryPath == null)
            {
                throw new Exception("Invalid file path. Archive could be corrupted.");
            }

            if (!Directory.Exists(Path.Combine(outputDirectoryPath, directoryPath)))
            {
                Directory.CreateDirectory(Path.Combine(outputDirectoryPath, directoryPath));
            }

            string outputPath = Path.Combine(outputDirectoryPath, archivedFile.Name);

            // Override existing file flag is set to true
            if (File.Exists(outputPath) && !overrideExisting)
            {
                return;
            }

            BinaryReader.BaseStream.Seek(archivedFile.Offset, SeekOrigin.Begin);
            byte[] data = BinaryReader.ReadBytes(archivedFile.Size);

            BinaryWriter binaryWriter = new BinaryWriter(File.OpenWrite(outputPath));

            binaryWriter.Write(DecryptFileData(data, archivedFile.Key));

            binaryWriter.Close();
        }



        /// <summary>
        /// Reads the contents of RGSSAD archive and populates ArchivedFiles property.
        /// </summary>
        private void ReadRGSSAD()
        {
            uint key = Constants.RGASSADv1Key;

            ArchivedFiles = new List<ArchivedFile>();

            BinaryReader.BaseStream.Seek(8, SeekOrigin.Begin);
            while (true)
            {
                ArchivedFile archivedFile = new ArchivedFile();

                int length = DecryptInteger(BinaryReader.ReadInt32(), ref key);
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
            long result = value ^ key;

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
            byte[] decryptedName = new byte[encryptedName.Length];

            for (int i = 0; i <= encryptedName.Length - 1; i++)
            {
                decryptedName[i] = (byte)(encryptedName[i] ^ (key & 0xff));

                key *= 7;
                key += 3;
            }

            string result = Encoding.UTF8.GetString(decryptedName);

            return result;
        }

        /// <summary>
        /// Decrypts the file from given bytes using given key.
        /// </summary>
        /// <param name="encryptedFileData">The encrypted file data.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        private byte[] DecryptFileData(byte[] encryptedFileData, uint key)
        {
            byte[] decryptedFileData = new byte[encryptedFileData.Length];

            uint tempKey = key;
            byte[] keyBytes = BitConverter.GetBytes(key);
            int j = 0;

            for (int i = 0; i <= encryptedFileData.Length - 1; i++)
            {
                if (j == 4)
                {
                    j = 0;
                    tempKey *= 7;
                    tempKey += 3;
                    keyBytes = BitConverter.GetBytes(tempKey);
                }

                decryptedFileData[i] = (byte)(encryptedFileData[i] ^ keyBytes[j]);

                j += 1;
            }

            return decryptedFileData;
        }
    }
}
