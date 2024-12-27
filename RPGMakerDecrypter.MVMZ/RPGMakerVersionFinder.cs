using System.IO;
using System.Linq;
using RPGMakerDecrypter.RGSSAD;

namespace RPGMakerDecrypter.MVMZ
{
    public class RPGMakerVersionFinder
    {
        public RPGMakerVersion FindVersion(string inputPath)
        {
            if (!Directory.Exists(inputPath))
            {
                return RPGMakerVersion.Unknown;
            }
            
            var directoryInfo = new DirectoryInfo(inputPath);

            if (directoryInfo.GetDirectories().Any(d => d.Name == "www"))
            {
                return RPGMakerVersion.MV;
            }

            return RPGMakerVersion.MZ;
        }
    }
}