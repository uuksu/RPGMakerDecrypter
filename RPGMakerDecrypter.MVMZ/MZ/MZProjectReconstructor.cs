using System;
using System.IO;

namespace RPGMakerDecrypter.MVMZ.MZ
{
    public class MZProjectReconstructor : ProjectReconstructor
    {
        protected override void CreateProjectFile(string outputPath)
        {
            File.WriteAllText(
                Path.Combine(outputPath, Constants.MZProjectFileName),
                Constants.MZProjectFileContent);
        }
    }
}