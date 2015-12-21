using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RPGMakerDecrypter.Decrypter;

namespace RPGMakerDecrypter.Tests
{
    [TestClass]
    public class RGSSADv3Tests
    {
        [TestMethod]
        public void CorrectAmountOfArchivedFilesIsReadFromVxAceArchive()
        {
            FileHelpers.CopyArchives();

            RGSSADv3 rgssad = new RGSSADv3(Path.Combine(FileHelpers.TempDirectoryPath, Constants.RpgMakerVxAceArchiveName));

            Assert.AreEqual(16, rgssad.ArchivedFiles.Count);

            rgssad.Dispose();

            FileHelpers.Cleanup();
        }

        [TestMethod]
        public void CorrectFileNamesAreReadFromVxAceArchive()
        {
            FileHelpers.CopyArchives();

            RGSSADv3 rgssad = new RGSSADv3(Path.Combine(FileHelpers.TempDirectoryPath, Constants.RpgMakerVxAceArchiveName));

            // Verified with Falos RPG Maker Decrypter
            Assert.AreEqual(@"Data\Actors.rvdata2", rgssad.ArchivedFiles[0].Name);
            Assert.AreEqual(@"Data\Animations.rvdata2", rgssad.ArchivedFiles[1].Name);
            Assert.AreEqual(@"Data\Armors.rvdata2", rgssad.ArchivedFiles[2].Name);

            rgssad.Dispose();

            FileHelpers.Cleanup();
        }

        [TestMethod]
        public void CorrectOffsetsAreReadFromVxAceArchive()
        {
            FileHelpers.CopyArchives();

            RGSSADv3 rgssad = new RGSSADv3(Path.Combine(FileHelpers.TempDirectoryPath, Constants.RpgMakerVxAceArchiveName));

            // Verified with Falos RPG Maker Decrypter
            Assert.AreEqual(605, rgssad.ArchivedFiles[0].Offset);
            Assert.AreEqual(3637, rgssad.ArchivedFiles[1].Offset);
            Assert.AreEqual(222096, rgssad.ArchivedFiles[2].Offset);

            rgssad.Dispose();

            FileHelpers.Cleanup();
        }

        [TestMethod]
        public void CorrectSizesAreReadFromVxAceArchive()
        {
            FileHelpers.CopyArchives();

            RGSSADv3 rgssad = new RGSSADv3(Path.Combine(FileHelpers.TempDirectoryPath, Constants.RpgMakerVxAceArchiveName));

            // Verified with Falos RPG Maker Decrypter
            Assert.AreEqual(3032, rgssad.ArchivedFiles[0].Size);
            Assert.AreEqual(218459, rgssad.ArchivedFiles[1].Size);
            Assert.AreEqual(11472, rgssad.ArchivedFiles[2].Size);

            rgssad.Dispose();

            FileHelpers.Cleanup();
        }

        [TestMethod]
        public void CorrectKeysAreReadFromVxAceArchive()
        {
            FileHelpers.CopyArchives();

            RGSSADv3 rgssad = new RGSSADv3(Path.Combine(FileHelpers.TempDirectoryPath, Constants.RpgMakerVxAceArchiveName));

            // Verified with Falos RPG Maker Decrypter
            Assert.AreEqual((uint)0x00000029, rgssad.ArchivedFiles[0].Key);
            Assert.AreEqual((uint)0x00004823, rgssad.ArchivedFiles[1].Key);
            Assert.AreEqual((uint)0x000018BE, rgssad.ArchivedFiles[2].Key);

            rgssad.Dispose();

            FileHelpers.Cleanup();
        }
    }
}
