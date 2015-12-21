using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;

namespace RPGMakerDecrypter.Cli
{
    class CommandLineOptions
    {
        [ValueList(typeof(List<string>), MaximumElements = 1)]
        public IList<string> InputPaths { get; set; }

        [Option('o', "output", Required = true, HelpText = "Output directory path.")]
        public string OutputDirectoryPath { get; set; }

        [Option('p', "project-file", Required = false, HelpText = "If set to true then generates project file.")]
        public bool GenerateProjectFile { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            Assembly entryAssembly = Assembly.GetEntryAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(entryAssembly.Location);

            HelpText help = new HelpText
            {
                Heading = new HeadingInfo("RPG Maker Decrypter", fvi.FileVersion),
                Copyright = new CopyrightInfo("Mikko Uuksulainen", 2015),
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true
            };

            help.AddPreOptionsLine(Environment.NewLine);
            help.AddPreOptionsLine("Usage: RPGMakerDecrypter-cli <inputPath> [options]");
            help.AddOptions(this);

            return help;
        }
    }
}
