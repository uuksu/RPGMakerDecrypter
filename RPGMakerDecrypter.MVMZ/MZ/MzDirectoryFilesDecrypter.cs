namespace RPGMakerDecrypter.MVMZ.MZ
{
    public class MzDirectoryFilesDecrypter : DirectoryFilesDecrypter
    {
        public void DecryptFiles(byte[] key, string inputPath, bool deleteEncrypted, bool overwrite)
        {
            Decrypt(key, inputPath, Constants.MZFileExtensionMaps, deleteEncrypted, overwrite);
        }
    }
}