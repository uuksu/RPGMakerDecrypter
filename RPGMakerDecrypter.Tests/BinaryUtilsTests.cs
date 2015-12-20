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
        private static string tempDirectoryPath = "Temp";

        private void CopyArchives()
        {
            Directory.CreateDirectory(tempDirectoryPath);

            File.Copy(Path.Combine("../../EncryptedArchives", Constants.RpgMakerXpArchiveName), Path.Combine(tempDirectoryPath, Constants.RpgMakerXpArchiveName));
            File.Copy(Path.Combine("../../EncryptedArchives", Constants.RpgMakerVxArchiveName), Path.Combine(tempDirectoryPath, Constants.RpgMakerVxArchiveName));
            File.Copy(Path.Combine("../../EncryptedArchives", Constants.RpgMakerVxAceArchiveName), Path.Combine(tempDirectoryPath, Constants.RpgMakerVxAceArchiveName));
        }

        private void Cleanup()
        {
            File.Delete(Path.Combine(tempDirectoryPath, Constants.RpgMakerXpArchiveName));
            File.Delete(Path.Combine(tempDirectoryPath, Constants.RpgMakerVxArchiveName));
            File.Delete(Path.Combine(tempDirectoryPath, Constants.RpgMakerVxAceArchiveName));

            Directory.Delete(tempDirectoryPath);
        }

        [TestMethod]
        public void RPGMakerXpArchiveVersionIsOne()
        {
            CopyArchives();

            using (BinaryReader binaryReader = new BinaryReader(new FileStream(Path.Combine(tempDirectoryPath, Constants.RpgMakerXpArchiveName), FileMode.Open)))
            {
                string s = BinaryUtils.ReadCString(binaryReader, 7);
                byte version = binaryReader.ReadByte();

                Assert.AreEqual(version, 1);
            }

            using (BinaryReader binaryReader = new BinaryReader(new FileStream(Path.Combine(tempDirectoryPath, Constants.RpgMakerVxArchiveName), FileMode.Open)))
            {
                string s = BinaryUtils.ReadCString(binaryReader, 7);
                byte version = binaryReader.ReadByte();

                Assert.AreEqual(version, 1);
            }

            using (BinaryReader binaryReader = new BinaryReader(new FileStream(Path.Combine(tempDirectoryPath, Constants.RpgMakerVxAceArchiveName), FileMode.Open)))
            {
                string s = BinaryUtils.ReadCString(binaryReader, 7);
                byte version = binaryReader.ReadByte();

                Assert.AreEqual(version, 3);
            }

            Cleanup();
        }

        [TestMethod]
        public void RPGMakerVxArchiveVersionIsOne()
        {
            CopyArchives();

            using (BinaryReader binaryReader = new BinaryReader(new FileStream(Path.Combine(tempDirectoryPath, Constants.RpgMakerVxArchiveName), FileMode.Open)))
            {
                string s = BinaryUtils.ReadCString(binaryReader, 7);
                byte version = binaryReader.ReadByte();

                Assert.AreEqual(version, 1);
            }

            Cleanup();
        }

        [TestMethod]
        public void RPGMakerVxAceArchiveVersionIsThree()
        {
            CopyArchives();

            using (BinaryReader binaryReader = new BinaryReader(new FileStream(Path.Combine(tempDirectoryPath, Constants.RpgMakerVxAceArchiveName), FileMode.Open)))
            {
                string s = BinaryUtils.ReadCString(binaryReader, 7);
                byte version = binaryReader.ReadByte();

                Assert.AreEqual(version, 3);
            }

            Cleanup();
        }

        [TestMethod]
        public void ReadCStringReturnsCorrectHeaderForArchives()
        {
            CopyArchives();

            using (BinaryReader binaryReader = new BinaryReader(new FileStream(Path.Combine(tempDirectoryPath, Constants.RpgMakerXpArchiveName), FileMode.Open)))
            {
                string s = BinaryUtils.ReadCString(binaryReader, 7);
                Assert.AreEqual(s, Constants.RGSSADHeader);
            }

            using (BinaryReader binaryReader = new BinaryReader(new FileStream(Path.Combine(tempDirectoryPath, Constants.RpgMakerVxArchiveName), FileMode.Open)))
            {
                string s = BinaryUtils.ReadCString(binaryReader, 7);
                Assert.AreEqual(s, Constants.RGSSADHeader);
            }

            using (BinaryReader binaryReader = new BinaryReader(new FileStream(Path.Combine(tempDirectoryPath, Constants.RpgMakerVxAceArchiveName), FileMode.Open)))
            {
                string s = BinaryUtils.ReadCString(binaryReader, 7);
                Assert.AreEqual(s, Constants.RGSSADHeader);
            }

            Cleanup();
        }


    }
}