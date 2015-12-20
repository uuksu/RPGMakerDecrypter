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

        public void Dispose()
        {
            BinaryReader.Close();
            BinaryReader.Dispose();
        }
    }
}
