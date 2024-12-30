using System.IO;
using System.Linq;

namespace RPGMakerDecrypter.MVMZ.MV
{
    public class MVProjectReconstructor : ProjectReconstructor
    {
        public override void Reconstruct(string deploymentPath, string outputPath)
        {
            // Windows, Linux and Web and Android/iOS deployments have a separate www-directory
            var dataFilesPath = Path.Combine(deploymentPath, "www");

            // MacOS deployments have a different structure that needs to be handled separately
            var macOSBundleDirectory = Directory.GetDirectories(deploymentPath, "*.app", 
                SearchOption.TopDirectoryOnly).SingleOrDefault();
            
            if (!string.IsNullOrWhiteSpace(macOSBundleDirectory))
            {
                dataFilesPath = Path.Combine(macOSBundleDirectory, Constants.MacOSBundleDirectory);
            }
            
            // www-directory essentially contains the project files
            base.Reconstruct(dataFilesPath, outputPath);
        }

        protected override void CreateProjectFile(string outputPath)
        {
            File.WriteAllText(
                Path.Combine(outputPath, Constants.MVProjectFileName),
                Constants.MVProjectFileContent);
        }
    }
}