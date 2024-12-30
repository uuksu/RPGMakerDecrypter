using System.IO;
using System.Linq;

namespace RPGMakerDecrypter.MVMZ.MZ
{
    public class MZProjectReconstructor : ProjectReconstructor
    {
        public override void Reconstruct(string deploymentPath, string outputPath)
        {
            // Windows, Linux and Web and Android/iOS deployments all files are in root of the deployment
            var dataFilesPath = deploymentPath;

            // MacOS deployments have a different structure that needs to be handled separately
            var macOSBundleDirectory = Directory.GetDirectories(deploymentPath, "*.app", 
                SearchOption.TopDirectoryOnly).SingleOrDefault();
            
            if (!string.IsNullOrWhiteSpace(macOSBundleDirectory))
            {
                dataFilesPath = Path.Combine(macOSBundleDirectory, Constants.MacOSBundleDirectory);
            }
            
            base.Reconstruct(dataFilesPath, outputPath);    
        }
        
        protected override void CreateProjectFile(string outputPath)
        {
            File.WriteAllText(
                Path.Combine(outputPath, Constants.MZProjectFileName),
                Constants.MZProjectFileContent);
        }
    }
}