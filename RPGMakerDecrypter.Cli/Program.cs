using System;
using System.IO;
using System.Linq;
using CommandLine;
using RPGMakerDecrypter.Common;
using RPGMakerDecrypter.Decrypter;
using RPGMakerDecrypter.Decrypter.Exceptions;

namespace RPGMakerDecrypter.Cli
{
    static class Program
    {
        private static CommandLineOptions _commandLineOptions;

        static void Main(string[] args)
        {
            var parsedResult = Parser.Default.ParseArguments<CommandLineOptions>(args);
            _commandLineOptions = parsedResult.Value;

            if (parsedResult.Errors.Any())
            {
                Environment.Exit(1);
            }

            if(!File.Exists(_commandLineOptions.InputPath))
            {
                Console.WriteLine($"RGSSAD file not found in path '{_commandLineOptions.InputPath}'");
                Environment.Exit(1);
            }

            var version = RGSSAD.GetRPGMakerVersion(_commandLineOptions.InputPath);

            if (version == RPGMakerVersion.Unknown)
            {
                Console.WriteLine("Unable to determinite RGSSAD RPG Maker version. " +
                    "Please rename RGSSAD file with a extension corresponding to version: " +
                    "XP: .rgssad, VX: .rgss2a, VX Ace: .rgss3a");
                Environment.Exit(1);
            }

            string outputDirectoryPath;

            if (_commandLineOptions.OutputDirectoryPath != null)
            {
                outputDirectoryPath = _commandLineOptions.OutputDirectoryPath;
            }
            else
            {
                var fi = new FileInfo(_commandLineOptions.InputPath);
                outputDirectoryPath = fi.DirectoryName;
            }

            try
            {
                switch (version)
                {
                    case RPGMakerVersion.Xp:
                    case RPGMakerVersion.Vx:
                        var rgssadv1 = new RGSSADv1(_commandLineOptions.InputPath);
                        rgssadv1.ExtractAllFiles(outputDirectoryPath, _commandLineOptions.Overwrite);
                        break;
                    case RPGMakerVersion.VxAce:
                        var rgssadv2 = new RGSSADv3(_commandLineOptions.InputPath);
                        rgssadv2.ExtractAllFiles(outputDirectoryPath, _commandLineOptions.Overwrite);
                        break;
                }
            }
            catch (InvalidArchiveException)
            {
                Console.WriteLine("Archive is invalid or corrupted. Reading failed.");
                Console.WriteLine("Please create a issue: https://github.com/uuksu/RPGMakerDecrypter/issues");
                Environment.Exit(1);
            }
            catch (UnsupportedArchiveException)
            {
                Console.WriteLine("Archive is not supported or it is corrupted.");
                Console.WriteLine("Please create a issue: https://github.com/uuksu/RPGMakerDecrypter/issues");
                Environment.Exit(1);
            }
            catch (Exception ex)
            {
                var logFilePath = ExceptionLogger.LogException(ex);
                Console.WriteLine("Unexpected error happened while trying to extract the archive.");
                Console.WriteLine($"Error log has been written to '{logFilePath}'");
                Console.WriteLine("Please create a issue and include the log contents there: https://github.com/uuksu/RPGMakerDecrypter/issues");
                Environment.Exit(1);
            }

            if (_commandLineOptions.GenerateProjectFile)
            {
                var outputSameAsArchivePath = new FileInfo(_commandLineOptions.InputPath).Directory.FullName == new DirectoryInfo(outputDirectoryPath).FullName;
                ProjectGenerator.GenerateProject(version, outputDirectoryPath, !outputSameAsArchivePath);
            }
        }
    }
}
