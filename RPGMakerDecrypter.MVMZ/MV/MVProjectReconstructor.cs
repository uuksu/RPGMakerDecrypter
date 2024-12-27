using System.IO;

namespace RPGMakerDecrypter.MVMZ.MV
{
    public class MVProjectReconstructor : ProjectReconstructor
    {
        public override void Reconstruct(string inputPath, string outputPath)
        {
            // www-directory essentially contains the project files
            base.Reconstruct(Path.Combine(inputPath, "www"), outputPath);
        }

        protected override void CreateProjectFile(string outputPath)
        {
            File.WriteAllText(
                Path.Combine(outputPath, Constants.MVProjectFileName),
                Constants.MVProjectFileContent);
        }
    }
}