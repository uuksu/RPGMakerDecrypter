using System.Security.Cryptography;
using NUnit.Framework;
using RPGMakerDecrypter.MVMZ;

namespace RPGMakerDecrypter.Tests;

public class FileDecrypterTests
{
    [Test]
    public void FilesDecryptsCorrectly()
    {
        FileHelpers.CopyEncryptedFiles();

        // Bytes of "12345" hashed to "827ccb0eea8a706c4c34a16891f84e7b"
        byte[] key =
        [
            130, 124, 203, 14, 234, 138, 112, 108, 76, 52, 161, 104, 145, 248, 78, 123
        ];
        
        var fileDecryptor = new FileDecrypter();
        
        var imageBytes = fileDecryptor.Decrypt(key, Path.Combine(FileHelpers.TempDirectoryPath, "Image"));
        var audioOrbisBytes = fileDecryptor.Decrypt(key, Path.Combine(FileHelpers.TempDirectoryPath, "AudioOrbis"));
        var audioMpegBytes = fileDecryptor.Decrypt(key, Path.Combine(FileHelpers.TempDirectoryPath, "AudioMpeg"));
        
        var imageSha1 = Convert.ToHexString(SHA1.HashData(imageBytes)).ToLower();
        var audioOrbisSha1 = Convert.ToHexString(SHA1.HashData(audioOrbisBytes)).ToLower();
        var audioMpegSha1 = Convert.ToHexString(SHA1.HashData(audioMpegBytes)).ToLower();
        
        Assert.That(imageSha1 == "1d47f411bf4f6df654398faeae8f944f954258bf", Is.True);
        Assert.That(audioOrbisSha1 == "90d87198849f7bdfdd4eacd611de3a2540925813", Is.True);
        Assert.That(audioMpegSha1 == "f59b4b2f293d1964d85bc1a2e96e345ba22996b4", Is.True);
        
        FileHelpers.CleanupEncryptedFiles();
    }
}