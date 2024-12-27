using System;
using System.IO;
using System.Security.AccessControl;
using NUnit;
using NUnit.Framework;
using RPGMakerDecrypter.RGSSAD;

namespace RPGMakerDecrypter.Tests
{
    public class BinaryUtilsTests
    {
        [Test]
        public void RPGMakerXpArchiveVersionIsOne()
        {
            FileHelpers.CopyArchives();

            using (var binaryReader = new BinaryReader(new FileStream(Path.Combine(FileHelpers.TempDirectoryPath, Constants.RpgMakerXpArchiveName), FileMode.Open)))
            {
                var s = BinaryUtils.ReadCString(binaryReader, 7);
                var version = binaryReader.ReadByte();

                Assert.That(version, Is.EqualTo(1));
            }

            FileHelpers.CleanupArchives();
        }

        [Test]
        public void RPGMakerVxArchiveVersionIsOne()
        {
            FileHelpers.CopyArchives();

            using (var binaryReader = new BinaryReader(new FileStream(Path.Combine(FileHelpers.TempDirectoryPath, Constants.RpgMakerVxArchiveName), FileMode.Open)))
            {
                var s = BinaryUtils.ReadCString(binaryReader, 7);
                var version = binaryReader.ReadByte();

                Assert.That(version, Is.EqualTo(1));
            }

            FileHelpers.CleanupArchives();
        }

        [Test]
        public void RPGMakerVxAceArchiveVersionIsThree()
        {
            FileHelpers.CopyArchives();

            using (var binaryReader = new BinaryReader(new FileStream(Path.Combine(FileHelpers.TempDirectoryPath, Constants.RpgMakerVxAceArchiveName), FileMode.Open)))
            {
                var s = BinaryUtils.ReadCString(binaryReader, 7);
                var version = binaryReader.ReadByte();

                Assert.That(version, Is.EqualTo(3));
            }

            FileHelpers.CleanupArchives();
        }

        [Test]
        public void ReadCStringReturnsCorrectHeaderForArchives()
        {
            FileHelpers.CopyArchives();

            using (var binaryReader = new BinaryReader(new FileStream(Path.Combine(FileHelpers.TempDirectoryPath, Constants.RpgMakerXpArchiveName), FileMode.Open)))
            {
                var s = BinaryUtils.ReadCString(binaryReader, 7);
                Assert.That(s, Is.EqualTo(Constants.RGSSADHeader));
            }

            using (var binaryReader = new BinaryReader(new FileStream(Path.Combine(FileHelpers.TempDirectoryPath, Constants.RpgMakerVxArchiveName), FileMode.Open)))
            {
                var s = BinaryUtils.ReadCString(binaryReader, 7);
                Assert.That(s, Is.EqualTo(Constants.RGSSADHeader));
            }

            using (var binaryReader = new BinaryReader(new FileStream(Path.Combine(FileHelpers.TempDirectoryPath, Constants.RpgMakerVxAceArchiveName), FileMode.Open)))
            {
                var s = BinaryUtils.ReadCString(binaryReader, 7);
                Assert.That(s, Is.EqualTo(Constants.RGSSADHeader));
            }

            FileHelpers.CleanupArchives();
        }


    }
}