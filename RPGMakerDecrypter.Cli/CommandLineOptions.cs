using CommandLine;

namespace RPGMakerDecrypter.Cli
{
    public class CommandLineOptions
    {
        [Value(0, Required = true, HelpText = "Path to the RGSSAD file.")]
        public string InputPath { get; set; }

        [Option('o', "output", Required = false, HelpText = "Optional output directory path.")]
        public string OutputDirectoryPath { get; set; }

        [Option('p', "reconstruct-project", Required = false, HelpText = "If set to true, tries to reconstruct the original project.")]
        public bool ReconstructProject { get; set; }
        
        
        [Option('w', "overwrite", Required = false, HelpText = "If set to true then it will overwrite files.")]
        public bool Overwrite { get; set; }
    }
}
