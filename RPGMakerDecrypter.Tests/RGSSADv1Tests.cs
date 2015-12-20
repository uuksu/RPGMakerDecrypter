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
    public class RGSSADv1Tests
    {
        [TestMethod]
        public void CorrectAmountOfArchivedFilesIsReadFromXpArchive()
        {
            FileHelpers.CopyArchives();

            RGSSADv1 rgssad = new RGSSADv1(Path.Combine(FileHelpers.TempDirectoryPath, Constants.RpgMakerXpArchiveName));

            Assert.AreEqual(16, rgssad.ArchivedFiles.Count);

            rgssad.Dispose();

            FileHelpers.Cleanup();
        }

        [TestMethod]
        public void CorrectFileNamesAreReadFromXpArchive()
        {
            FileHelpers.CopyArchives();

            RGSSADv1 rgssad = new RGSSADv1(Path.Combine(FileHelpers.TempDirectoryPath, Constants.RpgMakerXpArchiveName));

            // Verified with Falos RPG Maker Decrypter
            Assert.AreEqual(@"Data\Actors.rxdata", rgssad.ArchivedFiles[0].Name);
            Assert.AreEqual(@"Data\Animations.rxdata", rgssad.ArchivedFiles[1].Name);
            Assert.AreEqual(@"Data\Armors.rxdata", rgssad.ArchivedFiles[2].Name);

            rgssad.Dispose();

            FileHelpers.Cleanup();
        }

        [TestMethod]
        public void CorrectOffsetsAreReadFromXpArchive()
        {
            FileHelpers.CopyArchives();

            RGSSADv1 rgssad = new RGSSADv1(Path.Combine(FileHelpers.TempDirectoryPath, Constants.RpgMakerXpArchiveName));

            // Verified with Falos RPG Maker Decrypter
            Assert.AreEqual(34, rgssad.ArchivedFiles[0].Offset);
            Assert.AreEqual(11045, rgssad.ArchivedFiles[1].Offset);
            Assert.AreEqual(147314, rgssad.ArchivedFiles[2].Offset);

            rgssad.Dispose();

            FileHelpers.Cleanup();
        }

        [TestMethod]
        public void CorrectSizesAreReadFromXpArchive()
        {
            FileHelpers.CopyArchives();

            RGSSADv1 rgssad = new RGSSADv1(Path.Combine(FileHelpers.TempDirectoryPath, Constants.RpgMakerXpArchiveName));

            // Verified with Falos RPG Maker Decrypter
            Assert.AreEqual(10981, rgssad.ArchivedFiles[0].Size);
            Assert.AreEqual(136243, rgssad.ArchivedFiles[1].Size);
            Assert.AreEqual(4285, rgssad.ArchivedFiles[2].Size);

            rgssad.Dispose();

            FileHelpers.Cleanup();
        }
    }
}
