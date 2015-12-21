using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using RPGMakerDecrypter.Decrypter;

namespace RPGMakerDecrypter.Cli
{
    class Program
    {
        private static CommandLineOptions _commandLineOptions;

        static void Main(string[] args)
        {
            _commandLineOptions = new CommandLineOptions();

            if (Parser.Default.ParseArguments(args, _commandLineOptions) == false)
            {
                Environment.Exit(1);
            }

            if (_commandLineOptions.InputPaths.Count == 0)
            {
                Console.WriteLine("Please provide input path.");
                Environment.Exit(1);
            }

            RPGMakerVersion version = GetVersion(_commandLineOptions.InputPaths.First());

            if (version == RPGMakerVersion.None)
            {
                Console.WriteLine("Invalid input file.");
                Environment.Exit(1);
            }

            switch (version)
            {
                case RPGMakerVersion.Xp:
                case RPGMakerVersion.Vx:
                    RGSSADv1 rgssadv1 = new RGSSADv1(_commandLineOptions.InputPaths.First());
                    rgssadv1.ExtractAllFiles(_commandLineOptions.OutputDirectoryPath);
                    break;
                case RPGMakerVersion.VxAce:
                    RGSSADv3 rgssadv2 = new RGSSADv3(_commandLineOptions.InputPaths.First());
                    rgssadv2.ExtractAllFiles(_commandLineOptions.OutputDirectoryPath);
                    break;
            }

            if (_commandLineOptions.GenerateProjectFile)
            {
                GenerateProjectFile(version, _commandLineOptions.OutputDirectoryPath);
            }
        }

        static RPGMakerVersion GetVersion(string inputPath)
        {
            FileInfo fi = new FileInfo(inputPath);

            switch (fi.Name)
            {
                case Constants.RpgMakerXpArchiveName:
                    return RPGMakerVersion.Xp;
                case Constants.RpgMakerVxArchiveName:
                    return RPGMakerVersion.Vx;
                case Constants.RpgMakerVxAceArchiveName:
                    return RPGMakerVersion.VxAce;
                default:
                    return RPGMakerVersion.None;
            }
        }

        static void GenerateProjectFile(RPGMakerVersion version, string outputDirectoryPath)
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
