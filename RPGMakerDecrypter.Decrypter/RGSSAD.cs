﻿using System;
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
    public abstract class RGSSAD : IDisposable
    {
        protected readonly string FilePath;
        protected readonly BinaryReader BinaryReader;
        protected uint key;

        protected virtual int GetArchieveStartPosition() => 8;

        /// <summary>
        /// Reads the contents of RGSSAD archive and populates ArchivedFiles property.
        /// </summary>
        protected abstract void ReadRGSSAD();

        /// <summary>
        /// Reads the contents of RGSSAD archive for the key of decrypting.
        /// </summary>
        protected abstract void PrepareKey();

        /// <summary>
        /// Decrypts integer from given value.
        /// Proceeds key forward by calculating new value.
        /// </summary>
        /// <param name="value">Encrypted value</param>
        /// <returns>Decrypted integer</returns>
        protected abstract int DecryptInteger(int value);
        /// <summary>
        /// Decrypts file name from given bytes using given key.
        /// Proceeds key forward by calculating new value.
        /// </summary>
        /// <param name="encryptedName">Encrypted filename</param>
        /// <returns>Decrypted filename</returns>
        protected abstract string DecryptFilename(byte[] encryptedName);

        public List<ArchivedFile> ArchivedFiles { get; set; }

        public RGSSAD(BinaryReader inBinaryReader)
        {
            BinaryReader = inBinaryReader;
            BinaryReader.BaseStream.Seek(GetArchieveStartPosition(), SeekOrigin.Begin);
            PrepareKey();
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
        /// <param name="overrideExisting">If set to true, overrides existing files</param>
        /// <param name="createDirectory">If set to true, creates directory specified in encrypted file name</param>
        /// <exception cref="System.Exception">Invalid file path. Archive could be corrupted.</exception>
        public void ExtractFile(ArchivedFile archivedFile, string outputDirectoryPath, bool overrideExisting = false, bool createDirectory = true)
        {
            var platformSpecificArchiveFilePath = ArchivedFileNameUtils.GetPlatformSpecificPath(archivedFile.Name);

            string outputPath;

            if (createDirectory)
            {
                string directoryPath = Path.GetDirectoryName(platformSpecificArchiveFilePath);

                if (directoryPath == null)
                {
                    throw new Exception("Invalid file path. Archive could be corrupted.");
                }

                if (!Directory.Exists(Path.Combine(outputDirectoryPath, directoryPath)))
                {
                    Directory.CreateDirectory(Path.Combine(outputDirectoryPath, directoryPath));
                }

                outputPath = Path.Combine(outputDirectoryPath, platformSpecificArchiveFilePath);
            }
            else
            {
                string fileName = Path.GetFileName(platformSpecificArchiveFilePath);
                outputPath = Path.Combine(outputDirectoryPath, fileName);
            }

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

        /// <summary>
        /// Gets the RPG Maker version based on RGASSD file extension.
        /// </summary>
        /// <param name="inputPath">Path to RGSSAD file</param>
        public static RPGMakerVersion GetVersion(string inputPath)
        {
            FileInfo fi = new FileInfo(inputPath);

            switch (fi.Name)
            {
                case Constants.RpgMakerXpArchiveName:
                    return RPGMakerVersion.Xp;
                case Constants.RpgMakerVxArchiveName:
                    return RPGMakerVersion.Vx;
                case Constants.RpgMakerVxAceArchiveName:
                    return RPGMakerVersion.VxAce;
                default:
                    return RPGMakerVersion.Invalid;
            }
        }
    }
}
