using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using RPGMakerDecrypter.Decrypter;
using RPGMakerDecrypter.Decrypter.Exceptions;

namespace RPGMakerDecrypter.Cli
{
    static class Program
    {
        public enum PackVersion
        {
            Invalid = -1,
            Vx = 1,
            VxAce = 3,
            Fux2Pack = (int)('k'),
        }
        private static CommandLineOptions _commandLineOptions;

        static void Main(string[] args)
        {
            var parsedResult = Parser.Default.ParseArguments<CommandLineOptions>(args);
            _commandLineOptions = parsedResult.Value;

            if (parsedResult.Errors.Any())
            {
                Environment.Exit(1);
            }

            RPGMakerVersion version = RGSSAD.GetVersion(_commandLineOptions.InputPath);

            if (version == RPGMakerVersion.Invalid)
            {
                Console.WriteLine("Invalid input file.");
                Environment.Exit(1);
            }

            string outputDirectoryPath;

            if (_commandLineOptions.OutputDirectoryPath != null)
            {
                outputDirectoryPath = _commandLineOptions.OutputDirectoryPath;
            }
            else
            {
                FileInfo fi = new FileInfo(_commandLineOptions.InputPath);
                outputDirectoryPath = fi.DirectoryName;
            }

            try
            {
                var binaryReader = new BinaryReader(new FileStream(_commandLineOptions.InputPath, FileMode.Open));
                string header;
                try
                {
                    header = BinaryUtils.ReadString(binaryReader, 7);
                }
                catch (Exception)
                {
                    throw new InvalidArchiveException("Archive is in invalid format.");
                }

                if (!Constants.RGSSADHeader.Contains(header))
                {
                    throw new InvalidArchiveException("Header was not found for archive.");
                }
                int versionNumber = binaryReader.ReadByte();

                if (!Constants.SupportedRGSSVersions.Contains(versionNumber))
                {
                    versionNumber = -1;
                }

                binaryReader.BaseStream.Seek(0, SeekOrigin.Begin);

                switch ((PackVersion)versionNumber)
                {
                    case PackVersion.Vx:
                        var rgssadv1 = new RGSSADv1(binaryReader);
                        rgssadv1.ExtractAllFiles(outputDirectoryPath);
                        break;
                    case PackVersion.VxAce:
                        var rgssadv3 = new RGSSADv3(binaryReader);
                        rgssadv3.ExtractAllFiles(outputDirectoryPath);
                        break;
                    case PackVersion.Fux2Pack:
                        var rgssadv3Fux2 = new RGSSADv3Fux2Pack(binaryReader);
                        rgssadv3Fux2.ExtractAllFiles(outputDirectoryPath);
                        break;
                    default:
                        throw new UnsupportedArchiveException("Invalid version number from binary reader");
                }
                
            }
            catch (InvalidArchiveException)
            {
                Console.WriteLine("Archive is invalid or corrupted. Reading failed.");
                Environment.Exit(1);
            }
            catch (UnsupportedArchiveException)
            {
                Console.WriteLine("Archive is not supported or it is corrupted.");
                Environment.Exit(1);
            }
            catch (Exception)
            {
                Console.WriteLine("Something went wrong with reading or extraction. Archive is likely invalid or corrupted.");
                Environment.Exit(1);
            }

            if (_commandLineOptions.GenerateProjectFile)
            {
                ProjectGenerator.GenerateProject(version, outputDirectoryPath);
            }
        }
    }
}
