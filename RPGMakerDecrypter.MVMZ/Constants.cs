using System.Collections.Generic;
using System.IO;

namespace RPGMakerDecrypter.MVMZ
{
    public static class Constants
    {
        public static readonly Dictionary<string, string> MVFileExtensionMaps = new Dictionary<string, string>()
        {
            { ".rpgmvo", ".ogg" },
            { ".rpgmvp", ".png" },
            { ".rpgmvm", ".m4a" }
        };
        
        public const string MVProjectFileName = "Game.rpgproject";
        public const string MVProjectFileContent = "RPGMV 1.6.3";

        
        public static readonly Dictionary<string, string> MZFileExtensionMaps = new Dictionary<string, string>()
        {
            { ".ogg_", ".ogg" },
            { ".png_", ".png" },
            { ".m4a_", ".m4a" }
        };
        
        public const string MZProjectFileName = "game.rmmzproject";
        public const string MZProjectFileContent = "RPGMZ 1.8.0";
        
        public static readonly string MacOSBundleDirectory = Path.Combine("Contents", "Resources", "app.nw");
    }
}