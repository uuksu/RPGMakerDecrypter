using System.IO;
using System.Linq;
using RPGMakerDecrypter.RGSSAD;

namespace RPGMakerDecrypter.MVMZ
{
    public class RPGMakerVersionFinder
    {
        /// <summary>
        /// Attempts to determine the RPG Maker version based on the directory structure of the input path.
        /// Can detect only MV or MZ deployments.
        /// </summary>
        /// <param name="deploymentPath">The path to the RPG Maker deployment directory.</param>
        /// <returns>The RPG Maker version, or RPGMakerVersion.Unknown if the input path does not exist or its structure is unrecognized.</returns>
        public RPGMakerVersion FindVersion(string deploymentPath)
        {
            if (!Directory.Exists(deploymentPath))
            {
                return RPGMakerVersion.Unknown;
            }
            
            var directoryInfo = new DirectoryInfo(deploymentPath);

            // MV deployments have a separate www-directory that contains the data
            if (directoryInfo.GetDirectories().Any(d => d.Name == "www"))
            {
                return RPGMakerVersion.MV;
            }
            
            // MacOS deployments look same on both version, but they have a slightly different structure
            var macOSBundleDirectory = Directory.GetDirectories(deploymentPath, "*.app", SearchOption.TopDirectoryOnly).SingleOrDefault();
            if (!string.IsNullOrWhiteSpace(macOSBundleDirectory))
            {
                // Framework directory is missing from MV, it only exists in MZ
                if (Directory.Exists(Path.Combine(macOSBundleDirectory, "Contents", "Frameworks")))
                {
                    return RPGMakerVersion.MZ;
                }

                return RPGMakerVersion.MV;
            }

            // MZ deployments have the files in the root directory
            return RPGMakerVersion.MZ;
        }
    }
}