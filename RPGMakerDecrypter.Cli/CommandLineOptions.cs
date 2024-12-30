using CommandLine;

namespace RPGMakerDecrypter.Cli
{
    public class CommandLineOptions
    {
        [Value(0, Required = true, HelpText = "Path to the .rgssad, .rgss2a or .rgss3a file or to MV and MZ deployment directory.")]
        public string InputPath { get; set; }

        [Option('o', "output", Required = false, HelpText = "Optional output directory path. Required if --reconstruct-project is set to true and input path is a MV or MZ directory.")]
        public string OutputDirectoryPath { get; set; }

        [Option('p', "reconstruct-project", Required = false, HelpText = "If set to true, tries to reconstruct the original project.")]
        public bool ReconstructProject { get; set; }
        
        [Option('w', "overwrite", Required = false, HelpText = "If set to true, destination files are overwritten.")]
        public bool Overwrite { get; set; }
    }
}
