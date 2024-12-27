namespace RPGMakerDecrypter.MVMZ.MV
{
    public class MVDirectoryFilesDecryptor : DirectoryFilesDecryptor
    {
        public void DecryptFiles(byte[] key, string inputPath, bool deleteEncrypted, bool overwrite)
        {
            Decrypt(key, inputPath, Constants.MVFileExtensionMaps, deleteEncrypted, overwrite);
        }
    }
}