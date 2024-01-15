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
        public static void GenerateProject(RPGMakerVersion version, string outputDirectoryPath, bool overwrite)
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

            var projectFilePath = Path.Combine(outputDirectoryPath, $"Game.{projectFileExtension}");
            var iniFilePath = Path.Combine(outputDirectoryPath, "Game.ini");

            if(overwrite)
            {
                File.WriteAllText(projectFilePath, projectFileContent);
            }

            if(overwrite)
            {
                File.WriteAllText(iniFilePath, iniFileContent);
            }
        }
    }
}
