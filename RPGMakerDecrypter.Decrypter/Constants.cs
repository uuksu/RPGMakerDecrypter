using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGMakerDecrypter.Decrypter
{
    public class Constants
    {
        public static readonly string RpgMakerXpArchiveName = "Game.rgssad";
        public static readonly string RpgMakerVxArchiveName = "Game.rgss2a";
        public static readonly string RpgMakerVxAceArchiveName = "Game.rgss3a";

        public static readonly string RGSSADHeader = "RGSSAD";

        public const int RGASSDv1 = 1;
        public const int RGASSDv3 = 3;

        public static readonly int[] SupportedRGSSVersions = { RGASSDv1, RGASSDv3 };

        public static readonly uint RGASSADv1Key = 0xDEADCAFE;
    }
}
