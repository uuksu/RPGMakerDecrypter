namespace RPGMakerDecrypter.MVMZ.MV
{
    public class MvDirectoryFilesDecrypter : DirectoryFilesDecrypter
    {
        public void DecryptFiles(byte[] key, string inputPath, bool deleteEncrypted, bool overwrite)
        {
            Decrypt(key, inputPath, Constants.MVFileExtensionMaps, deleteEncrypted, overwrite);
        }
    }
}