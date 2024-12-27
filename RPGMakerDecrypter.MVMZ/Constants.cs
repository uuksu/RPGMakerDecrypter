using System.Collections.Generic;

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
        
        public static readonly Dictionary<string, string> MZFileExtensionMaps = new Dictionary<string, string>()
        {
            { ".ogg_", ".ogg" },
            { ".png_", ".png" },
            { ".m4a_", ".m4a" }
        };
        
        public const string MVProjectFileName = "Game.rpgproject";
        public const string MZProjectFileName = "game.rmmzproject";
        public const string MVProjectFileContent = "RPGMV 1.6.3";
        public const string MZProjectFileContent = "RPGMZ 1.8.0";
    }
}