using System;
using System.IO;
using System.Security.AccessControl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RPGMakerDecrypter.Decrypter;

namespace RPGMakerDecrypter.Tests
{
    [TestClass]
    public class BinaryUtilsTests
    {
        [TestMethod]
        public void RPGMakerXpArchiveVersionIsOne()
        {
            FileHelpers.CopyArchives();

            using (BinaryReader binaryReader = new BinaryReader(new FileStream(Path.Combine(FileHelpers.TempDirectoryPath, Constants.RpgMakerXpArchiveName), FileMode.Open)))
            {
                string s = BinaryUtils.ReadCString(binaryReader, 7);
                byte version = binaryReader.ReadByte();

                Assert.AreEqual(1, version);
            }

            FileHelpers.Cleanup();
        }

        [TestMethod]
        public void RPGMakerVxArchiveVersionIsOne()
        {
            FileHelpers.CopyArchives();

            using (BinaryReader binaryReader = new BinaryReader(new FileStream(Path.Combine(FileHelpers.TempDirectoryPath, Constants.RpgMakerVxArchiveName), FileMode.Open)))
            {
                string s = BinaryUtils.ReadCString(binaryReader, 7);
                byte version = binaryReader.ReadByte();

                Assert.AreEqual(1, version);
            }

            FileHelpers.Cleanup();
        }

        [TestMethod]
        public void RPGMakerVxAceArchiveVersionIsThree()
        {
            FileHelpers.CopyArchives();

            using (BinaryReader binaryReader = new BinaryReader(new FileStream(Path.Combine(FileHelpers.TempDirectoryPath, Constants.RpgMakerVxAceArchiveName), FileMode.Open)))
            {
                string s = BinaryUtils.ReadCString(binaryReader, 7);
                byte version = binaryReader.ReadByte();

                Assert.AreEqual(3, version);
            }

            FileHelpers.Cleanup();
        }

        [TestMethod]
        public void ReadCStringReturnsCorrectHeaderForArchives()
        {
            FileHelpers.CopyArchives();

            using (BinaryReader binaryReader = new BinaryReader(new FileStream(Path.Combine(FileHelpers.TempDirectoryPath, Constants.RpgMakerXpArchiveName), FileMode.Open)))
            {
                string s = BinaryUtils.ReadCString(binaryReader, 7);
                Assert.AreEqual(Constants.RGSSADHeader, s);
            }

            using (BinaryReader binaryReader = new BinaryReader(new FileStream(Path.Combine(FileHelpers.TempDirectoryPath, Constants.RpgMakerVxArchiveName), FileMode.Open)))
            {
                string s = BinaryUtils.ReadCString(binaryReader, 7);
                Assert.AreEqual(Constants.RGSSADHeader, s);
            }

            using (BinaryReader binaryReader = new BinaryReader(new FileStream(Path.Combine(FileHelpers.TempDirectoryPath, Constants.RpgMakerVxAceArchiveName), FileMode.Open)))
            {
                string s = BinaryUtils.ReadCString(binaryReader, 7);
                Assert.AreEqual(Constants.RGSSADHeader, s);
            }

            FileHelpers.Cleanup();
        }


    }
}