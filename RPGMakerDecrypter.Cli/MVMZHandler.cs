using System;
using System.IO;
using RPGMakerDecrypter.Cli.Exceptions;
using RPGMakerDecrypter.Common;
using RPGMakerDecrypter.MVMZ;
using RPGMakerDecrypter.MVMZ.Exceptions;
using RPGMakerDecrypter.MVMZ.MV;
using RPGMakerDecrypter.MVMZ.MZ;
using RPGMakerDecrypter.RGSSAD;

namespace RPGMakerDecrypter.Cli
{
    public class MVMZHandler
    {
        public void Handle(CommandLineOptions commandLineOptions, RPGMakerVersion version)
        {
            if (commandLineOptions.ReconstructProject &&
                string.IsNullOrWhiteSpace(commandLineOptions.OutputDirectoryPath))
            {
                throw new InvalidUsageException("MV and MZ project reconstruction requires an output path. Please specify a path with -o or --output option.");
            }

            if (Directory.Exists(commandLineOptions.OutputDirectoryPath) && !commandLineOptions.Overwrite)
            {
                throw new InvalidUsageException("Output directory already exists. Please specify a different path or use the -w or --overwrite option.");
            }

            try
            {
                var workingDirectoryPath = commandLineOptions.InputPath;
                var deleteEncrypted = false;

                if (commandLineOptions.ReconstructProject)
                {
                    switch (version)
                    {
                        case RPGMakerVersion.MV:
                            new MVProjectReconstructor().Reconstruct(
                                commandLineOptions.InputPath,
                                commandLineOptions.OutputDirectoryPath);
                            break;
                        case RPGMakerVersion.MZ:
                            new MZProjectReconstructor().Reconstruct(
                                commandLineOptions.InputPath,
                                commandLineOptions.OutputDirectoryPath);
                            break;
                    }

                    // Change working directory to the project directory where all the necessary files are too
                    workingDirectoryPath = commandLineOptions.OutputDirectoryPath;
                    deleteEncrypted = true;
                }

                var encryptionKeyFinder = new EncryptionKeyFinder();
                var encryptionKey = encryptionKeyFinder.FindKey(workingDirectoryPath);

                switch (version)
                {
                    case RPGMakerVersion.MV:
                        new MVDirectoryFilesDecryptor().DecryptFiles(
                            encryptionKey, 
                            workingDirectoryPath,
                            deleteEncrypted,
                            commandLineOptions.Overwrite);
                        break;
                    case RPGMakerVersion.MZ:
                        new MZDirectoryFilesDecryptor().DecryptFiles(
                            encryptionKey, 
                            workingDirectoryPath,
                            deleteEncrypted,
                            commandLineOptions.Overwrite);
                        break;
                }
            }
            catch (EncryptionKeyException ex)
            {
                Console.WriteLine(ex.Message);
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
        }   
    }
}

