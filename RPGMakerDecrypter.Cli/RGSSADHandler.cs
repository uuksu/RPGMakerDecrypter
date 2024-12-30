using System;
using System.IO;
using RPGMakerDecrypter.Cli.Exceptions;
using RPGMakerDecrypter.Common;
using RPGMakerDecrypter.RGSSAD;
using RPGMakerDecrypter.RGSSAD.Exceptions;

namespace RPGMakerDecrypter.Cli
{
    public class RGSSADHandler
    {
        public void Handle(CommandLineOptions commandLineOptions, RPGMakerVersion version)
        {
            string outputDirectoryPath;

            if (commandLineOptions.OutputDirectoryPath != null)
            {
                if (Directory.Exists(commandLineOptions.OutputDirectoryPath) && !commandLineOptions.Overwrite)
                {
                    throw new InvalidUsageException("Output directory already exists. Please specify a different path or use the -w or --overwrite option.");
                }
                
                if (!Directory.Exists(commandLineOptions.OutputDirectoryPath))
                {
                    Directory.CreateDirectory(commandLineOptions.OutputDirectoryPath);
                }
                
                outputDirectoryPath = commandLineOptions.OutputDirectoryPath;
            }
            else
            {
                var fi = new FileInfo(commandLineOptions.InputPath);
                outputDirectoryPath = fi.DirectoryName;
            }
            
            try
            {
                switch (version)
                {
                    case RPGMakerVersion.Xp:
                    case RPGMakerVersion.Vx:
                        var rgssadv1 = new RGSSADv1(commandLineOptions.InputPath);
                        rgssadv1.ExtractAllFiles(outputDirectoryPath, commandLineOptions.Overwrite);
                        break;
                    case RPGMakerVersion.VxAce:
                        var rgssadv2 = new RGSSADv3(commandLineOptions.InputPath);
                        rgssadv2.ExtractAllFiles(outputDirectoryPath, commandLineOptions.Overwrite);
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

            if (commandLineOptions.ReconstructProject)
            {
                var outputSameAsArchivePath = new FileInfo(commandLineOptions.InputPath).Directory.FullName == new DirectoryInfo(outputDirectoryPath).FullName;
                ProjectGenerator.GenerateProject(version, outputDirectoryPath, !outputSameAsArchivePath);
            }
        }   
    }
}

