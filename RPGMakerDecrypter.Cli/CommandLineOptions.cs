using CommandLine;

namespace RPGMakerDecrypter.Cli
{
    internal class CommandLineOptions
    {
        [Value(0, Required = true, HelpText = "Path to the RGSSAD file.")]
        public string InputPath { get; set; }

        [Option('o', "output", Required = false, HelpText = "Optional output directory path.")]
        public string OutputDirectoryPath { get; set; }

        [Option('p', "project-file", Required = false, HelpText = "If set to true then generates project file.")]
        public bool GenerateProjectFile { get; set; }
        
        
        [Option('w', "overwrite", Required = false, HelpText = "If set to true then it will overwrite files.")]
        public bool Overwrite { get; set; }
    }
}
