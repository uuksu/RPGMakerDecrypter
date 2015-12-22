using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGMakerDecrypter.Decrypter
{
    public static class ProjectGenerator
    {
        /// <summary>
        /// Generates the project file and ini for given RPG Maker.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="outputDirectoryPath">The output directory path.</param>
        public static void GenerateProject(RPGMakerVersion version, string outputDirectoryPath)
        {
            string projectFileContent = null;
            string projectFileExtension = null;
            string iniFileContent = null;

            switch (version)
            {
                case RPGMakerVersion.Xp:
                    projectFileContent = Constants.RpgMakerXpProjectFileContent;
                    projectFileExtension = Constants.RpgMakerXpProjectFileExtension;
                    iniFileContent = Constants.RPGMakerXpIniFileContents;
                    break;
                case RPGMakerVersion.Vx:
                    projectFileContent = Constants.RpgMakerVxProjectFileContent;
                    projectFileExtension = Constants.RpgMakerVxProjectFileExtension;
                    iniFileContent = Constants.RPGMakerVxIniFileContents;
                    break;
                case RPGMakerVersion.VxAce:
                    projectFileContent = Constants.RpgMakerVxAceProjectFileContent;
                    projectFileExtension = Constants.RpgMakerVxAceProjectFileExtension;
                    iniFileContent = Constants.RPGMakerVxAceIniFileContents;
                    break;
            }

            File.WriteAllText(Path.Combine(outputDirectoryPath, $"Game.{projectFileExtension}"), projectFileContent);
            File.WriteAllText(Path.Combine(outputDirectoryPath, "Game.ini"), iniFileContent);
        }
    }
}
