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
        private readonly string path;
        private readonly BinaryReader binaryReader;

        public RGSSAD(string path)
        {
            this.path = path;
            binaryReader = new BinaryReader(new FileStream(path, FileMode.Open));
        }

        /// <summary>
        /// Gets the version of RGSSAD.
        /// </summary>
        /// <param name="path">Path to RGSSAD archive</param>
        /// <returns></returns>
        /// <exception cref="InvalidArchiveException">
        /// Archive is in invalid format.
        /// or
        /// Header was not found for archive.
        /// </exception>
        /// <exception cref="UnsupportedArchiveException">$Archive version {result} is not supported.</exception>
        public int GetVersion()
        {
            string header;

            try
            {
                header = BinaryUtils.ReadCString(binaryReader, 7);
            }
            catch (Exception)
            {
                throw new InvalidArchiveException("Archive is in invalid format.");
            }

            if (header != Constants.RGSSADHeader)
            {
                throw new InvalidArchiveException("Header was not found for archive.");
            }

            int result = binaryReader.ReadByte();

            if (!Constants.SupportedRGSSVersions.Contains(result))
            {
                throw new UnsupportedArchiveException($"Archive version {result} is not supported.");
            }

            binaryReader.BaseStream.Seek(0, SeekOrigin.Begin);

            return result;
        }

        public void Dispose()
        {
            binaryReader.Close();
            binaryReader.Dispose();
        }
    }
}
