using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using RPGMakerDecrypter.RGSSAD;

namespace RPGMakerDecrypter.Tests
{
    public class RGSSADv3Tests
    {
        [Test]
        public void CorrectAmountOfArchivedFilesIsReadFromVxAceArchive()
        {
            FileHelpers.CopyArchives();

            var rgssad = new RGSSADv3(Path.Combine(FileHelpers.TempDirectoryPath, Constants.RpgMakerVxAceArchiveName));

            Assert.That(rgssad.ArchivedFiles.Count, Is.EqualTo(16));

            rgssad.Dispose();

            FileHelpers.Cleanup();
        }

        [Test]
        public void CorrectFileNamesAreReadFromVxAceArchive()
        {
            FileHelpers.CopyArchives();

            var rgssad = new RGSSADv3(Path.Combine(FileHelpers.TempDirectoryPath, Constants.RpgMakerVxAceArchiveName));

            // Verified with Falos RPG Maker Decrypter
            Assert.That(rgssad.ArchivedFiles[0].Name, Is.EqualTo(@"Data\Actors.rvdata2"));
            Assert.That(rgssad.ArchivedFiles[1].Name, Is.EqualTo(@"Data\Animations.rvdata2"));
            Assert.That(rgssad.ArchivedFiles[2].Name, Is.EqualTo(@"Data\Armors.rvdata2"));

            rgssad.Dispose();

            FileHelpers.Cleanup();
        }

        [Test]
        public void CorrectOffsetsAreReadFromVxAceArchive()
        {
            FileHelpers.CopyArchives();

            var rgssad = new RGSSADv3(Path.Combine(FileHelpers.TempDirectoryPath, Constants.RpgMakerVxAceArchiveName));

            // Verified with Falos RPG Maker Decrypter
            Assert.That(rgssad.ArchivedFiles[0].Offset, Is.EqualTo(605));
            Assert.That(rgssad.ArchivedFiles[1].Offset, Is.EqualTo(3637));
            Assert.That(rgssad.ArchivedFiles[2].Offset, Is.EqualTo(222096));

            rgssad.Dispose();

            FileHelpers.Cleanup();
        }

        [Test]
        public void CorrectSizesAreReadFromVxAceArchive()
        {
            FileHelpers.CopyArchives();

            var rgssad = new RGSSADv3(Path.Combine(FileHelpers.TempDirectoryPath, Constants.RpgMakerVxAceArchiveName));

            // Verified with Falos RPG Maker Decrypter
            Assert.That(rgssad.ArchivedFiles[0].Size, Is.EqualTo(3032));
            Assert.That(rgssad.ArchivedFiles[1].Size, Is.EqualTo(218459));
            Assert.That(rgssad.ArchivedFiles[2].Size, Is.EqualTo(11472));

            rgssad.Dispose();

            FileHelpers.Cleanup();
        }

        [Test]
        public void CorrectKeysAreReadFromVxAceArchive()
        {
            FileHelpers.CopyArchives();

            var rgssad = new RGSSADv3(Path.Combine(FileHelpers.TempDirectoryPath, Constants.RpgMakerVxAceArchiveName));

            // Verified with Falos RPG Maker Decrypter
            Assert.That(rgssad.ArchivedFiles[0].Key, Is.EqualTo((uint)0x00000029));
            Assert.That(rgssad.ArchivedFiles[1].Key, Is.EqualTo((uint)0x00004823));
            Assert.That(rgssad.ArchivedFiles[2].Key, Is.EqualTo((uint)0x000018BE));

            rgssad.Dispose();

            FileHelpers.Cleanup();
        }
    }
}
