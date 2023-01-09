using System;
using System.IO;
using System.Security.AccessControl;
using NUnit;
using NUnit.Framework;
using RPGMakerDecrypter.Decrypter;

namespace RPGMakerDecrypter.Tests
{
    public class BinaryUtilsTests
    {
        [Test]
        public void RPGMakerXpArchiveVersionIsOne()
        {
            FileHelpers.CopyArchives();

            using (BinaryReader binaryReader = new BinaryReader(new FileStream(Path.Combine(FileHelpers.TempDirectoryPath, Constants.RpgMakerXpArchiveName), FileMode.Open)))
            {
                string s = BinaryUtils.ReadCString(binaryReader, 7);
                byte version = binaryReader.ReadByte();

                Assert.That(version, Is.EqualTo(1));
            }

            FileHelpers.Cleanup();
        }

        [Test]
        public void RPGMakerVxArchiveVersionIsOne()
        {
            FileHelpers.CopyArchives();

            using (BinaryReader binaryReader = new BinaryReader(new FileStream(Path.Combine(FileHelpers.TempDirectoryPath, Constants.RpgMakerVxArchiveName), FileMode.Open)))
            {
                string s = BinaryUtils.ReadCString(binaryReader, 7);
                byte version = binaryReader.ReadByte();

                Assert.That(version, Is.EqualTo(1));
            }

            FileHelpers.Cleanup();
        }

        [Test]
        public void RPGMakerVxAceArchiveVersionIsThree()
        {
            FileHelpers.CopyArchives();

            using (BinaryReader binaryReader = new BinaryReader(new FileStream(Path.Combine(FileHelpers.TempDirectoryPath, Constants.RpgMakerVxAceArchiveName), FileMode.Open)))
            {
                string s = BinaryUtils.ReadCString(binaryReader, 7);
                byte version = binaryReader.ReadByte();

                Assert.That(version, Is.EqualTo(3));
            }

            FileHelpers.Cleanup();
        }

        [Test]
        public void ReadCStringReturnsCorrectHeaderForArchives()
        {
            FileHelpers.CopyArchives();

            using (BinaryReader binaryReader = new BinaryReader(new FileStream(Path.Combine(FileHelpers.TempDirectoryPath, Constants.RpgMakerXpArchiveName), FileMode.Open)))
            {
                string s = BinaryUtils.ReadCString(binaryReader, 7);
                Assert.That(s, Is.EqualTo(Constants.RGSSADHeader));
            }

            using (BinaryReader binaryReader = new BinaryReader(new FileStream(Path.Combine(FileHelpers.TempDirectoryPath, Constants.RpgMakerVxArchiveName), FileMode.Open)))
            {
                string s = BinaryUtils.ReadCString(binaryReader, 7);
                Assert.That(s, Is.EqualTo(Constants.RGSSADHeader));
            }

            using (BinaryReader binaryReader = new BinaryReader(new FileStream(Path.Combine(FileHelpers.TempDirectoryPath, Constants.RpgMakerVxAceArchiveName), FileMode.Open)))
            {
                string s = BinaryUtils.ReadCString(binaryReader, 7);
                Assert.That(s, Is.EqualTo(Constants.RGSSADHeader));
            }

            FileHelpers.Cleanup();
        }


    }
}