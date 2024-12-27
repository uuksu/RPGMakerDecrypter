using System;
using System.Linq;
using CommandLine;
using RPGMakerDecrypter.Cli.Exceptions;
using RPGMakerDecrypter.MVMZ;
using RPGMakerDecrypter.RGSSAD;

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

            var version = RGSSAD.RGSSAD.GetRPGMakerVersion(_commandLineOptions.InputPath);
            if (version == RPGMakerVersion.Unknown)
            {
                var mvMzVersionFinder = new RPGMakerVersionFinder();
                version = mvMzVersionFinder.FindVersion(_commandLineOptions.InputPath);
            }

            try
            {
                switch (version)
                {
                    case RPGMakerVersion.Xp:
                    case RPGMakerVersion.Vx:
                    case RPGMakerVersion.VxAce:
                        new RGSSADHandler().Handle(_commandLineOptions, version);
                        break;
                    case RPGMakerVersion.MV:
                    case RPGMakerVersion.MZ:
                        new MVMZHandler().Handle(_commandLineOptions, version);
                        break;
                    case RPGMakerVersion.Unknown:
                    default:
                        Console.WriteLine("Unable to determinite RPG Maker version. " +
                                          "Please rename RGSSAD file with a extension corresponding to version: " +
                                          "XP: .rgssad, VX: .rgss2a, VX Ace: .rgss3a " +
                                          "or point to MZ or MV directory (.");
                        Environment.Exit(1);
                        break;
                } 
            } catch (InvalidUsageException ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(1);
            }
        }
    }
}
