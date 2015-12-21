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
    /// Represents RPG Maker RGSS Encrypted Archive.
    /// </summary>
    public class RGSSAD : IDisposable
    {
        protected readonly string FilePath;
        protected readonly BinaryReader BinaryReader;

        public List<ArchivedFile> ArchivedFiles { get; set; }

        public RGSSAD(string filePath)
        {
            this.FilePath = filePath;
            BinaryReader = new BinaryReader(new FileStream(filePath, FileMode.Open));
        }

        /// <summary>
        /// Gets the version of RGSSAD.
        /// </summary>
        /// <param name="path">FilePath to RGSSAD archive</param>
        /// <returns></returns>
        /// <exception cref="InvalidArchiveException">
        /// Archive is in invalid format.
        /// or
        /// Header was not found for archive.
        /// </exception>
        public int GetVersion()
        {
            string header;

            try
            {
                header = BinaryUtils.ReadCString(BinaryReader, 7);
            }
            catch (Exception)
            {
                throw new InvalidArchiveException("Archive is in invalid format.");
            }

            if (header != Constants.RGSSADHeader)
            {
                throw new InvalidArchiveException("Header was not found for archive.");
            }

            int result = BinaryReader.ReadByte();

            if (!Constants.SupportedRGSSVersions.Contains(result))
            {
                result =  -1;
            }

            BinaryReader.BaseStream.Seek(0, SeekOrigin.Begin);

            return result;
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

        public void Dispose()
        {
            BinaryReader.Close();
            BinaryReader.Dispose();
        }
    }
}
