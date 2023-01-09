using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using RPGMakerDecrypter.Decrypter;

namespace RPGMakerDecrypter.Tests
{
    public class RGSSADv1Tests
    {
        [Test]
        public void CorrectAmountOfArchivedFilesIsReadFromXpArchive()
        {
            FileHelpers.CopyArchives();

            RGSSADv1 rgssad = new RGSSADv1(Path.Combine(FileHelpers.TempDirectoryPath, Constants.RpgMakerXpArchiveName));

            Assert.That(rgssad.ArchivedFiles.Count, Is.EqualTo(16));

            rgssad.Dispose();

            FileHelpers.Cleanup();
        }

        [Test]
        public void CorrectFileNamesAreReadFromXpArchive()
        {
            FileHelpers.CopyArchives();

            RGSSADv1 rgssad = new RGSSADv1(Path.Combine(FileHelpers.TempDirectoryPath, Constants.RpgMakerXpArchiveName));

            // Verified with Falos RPG Maker Decrypter
            Assert.That(rgssad.ArchivedFiles[0].Name, Is.EqualTo(@"Data\Actors.rxdata"));
            Assert.That(rgssad.ArchivedFiles[1].Name, Is.EqualTo(@"Data\Animations.rxdata"));
            Assert.That(rgssad.ArchivedFiles[2].Name, Is.EqualTo(@"Data\Armors.rxdata"));

            rgssad.Dispose();

            FileHelpers.Cleanup();
        }

        [Test]
        public void CorrectOffsetsAreReadFromXpArchive()
        {
            FileHelpers.CopyArchives();

            RGSSADv1 rgssad = new RGSSADv1(Path.Combine(FileHelpers.TempDirectoryPath, Constants.RpgMakerXpArchiveName));

            // Verified with Falos RPG Maker Decrypter
            Assert.That(rgssad.ArchivedFiles[0].Offset, Is.EqualTo(34));
            Assert.That(rgssad.ArchivedFiles[1].Offset, Is.EqualTo(11045));
            Assert.That(rgssad.ArchivedFiles[2].Offset, Is.EqualTo(147314));

            rgssad.Dispose();

            FileHelpers.Cleanup();
        }

        [Test]
        public void CorrectSizesAreReadFromXpArchive()
        {
            FileHelpers.CopyArchives();

            RGSSADv1 rgssad = new RGSSADv1(Path.Combine(FileHelpers.TempDirectoryPath, Constants.RpgMakerXpArchiveName));

            // Verified with Falos RPG Maker Decrypter
            Assert.That(rgssad.ArchivedFiles[0].Size, Is.EqualTo(10981));
            Assert.That(rgssad.ArchivedFiles[1].Size, Is.EqualTo(136243));
            Assert.That(rgssad.ArchivedFiles[2].Size, Is.EqualTo(4285));

            rgssad.Dispose();

            FileHelpers.Cleanup();
        }

        [Test]
        public void CorrectKeysAreReadFromXpArchive()
        {
            FileHelpers.CopyArchives();

            RGSSADv1 rgssad = new RGSSADv1(Path.Combine(FileHelpers.TempDirectoryPath, Constants.RpgMakerXpArchiveName));

            // Verified with Falos RPG Maker Decrypter
            Assert.That(rgssad.ArchivedFiles[0].Key, Is.EqualTo((uint)0x7B7448AE));
            Assert.That(rgssad.ArchivedFiles[1].Key, Is.EqualTo((uint)0x366D564E));
            Assert.That(rgssad.ArchivedFiles[2].Key, Is.EqualTo((uint)0x222699FE));

            rgssad.Dispose();

            FileHelpers.Cleanup();
        }

        [Test]
        public void CorrectFileNamesAreReadFromVxArchive()
        {
            FileHelpers.CopyArchives();

            RGSSADv1 rgssad = new RGSSADv1(Path.Combine(FileHelpers.TempDirectoryPath, Constants.RpgMakerVxArchiveName));

            // Verified with Falos RPG Maker Decrypter
            Assert.That(rgssad.ArchivedFiles[0].Name, Is.EqualTo(@"Data\Actors.rvdata"));
            Assert.That(rgssad.ArchivedFiles[1].Name, Is.EqualTo(@"Data\Animations.rvdata"));
            Assert.That(rgssad.ArchivedFiles[2].Name, Is.EqualTo(@"Data\Areas.rvdata"));

            rgssad.Dispose();

            FileHelpers.Cleanup();
        }

        [Test]
        public void CorrectOffsetsAreReadFromVxArchive()
        {
            FileHelpers.CopyArchives();

            RGSSADv1 rgssad = new RGSSADv1(Path.Combine(FileHelpers.TempDirectoryPath, Constants.RpgMakerVxArchiveName));

            // Verified with Falos RPG Maker Decrypter
            Assert.That(rgssad.ArchivedFiles[0].Offset, Is.EqualTo(34));
            Assert.That(rgssad.ArchivedFiles[1].Offset, Is.EqualTo(10951));
            Assert.That(rgssad.ArchivedFiles[2].Offset, Is.EqualTo(139280));

            rgssad.Dispose();

            FileHelpers.Cleanup();
        }

        [Test]
        public void CorrectSizesAreReadFromVxArchive()
        {
            FileHelpers.CopyArchives();

            RGSSADv1 rgssad = new RGSSADv1(Path.Combine(FileHelpers.TempDirectoryPath, Constants.RpgMakerVxArchiveName));

            // Verified with Falos RPG Maker Decrypter
            Assert.That(rgssad.ArchivedFiles[0].Size, Is.EqualTo(10887));
            Assert.That(rgssad.ArchivedFiles[1].Size, Is.EqualTo(128304));
            Assert.That(rgssad.ArchivedFiles[2].Size, Is.EqualTo(4));

            rgssad.Dispose();

            FileHelpers.Cleanup();
        }

        [Test]
        public void CorrectKeysAreReadFromVxArchive()
        {
            FileHelpers.CopyArchives();

            RGSSADv1 rgssad = new RGSSADv1(Path.Combine(FileHelpers.TempDirectoryPath, Constants.RpgMakerVxArchiveName));

            // Verified with Falos RPG Maker Decrypter
            Assert.That(rgssad.ArchivedFiles[0].Key, Is.EqualTo((uint)0x7B7448AE));
            Assert.That(rgssad.ArchivedFiles[1].Key, Is.EqualTo((uint)0x366D564E));
            Assert.That(rgssad.ArchivedFiles[2].Key, Is.EqualTo((uint)0x04E0F16D));

            rgssad.Dispose();

            FileHelpers.Cleanup();
        }


    }
}
