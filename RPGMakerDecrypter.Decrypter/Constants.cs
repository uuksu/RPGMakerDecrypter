using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGMakerDecrypter.Decrypter
{
    public class Constants
    {
        public const string RpgMakerXpArchiveName = "Game.rgssad";
        public const string RpgMakerVxArchiveName = "Game.rgss2a";
        public const string RpgMakerVxAceArchiveName = "Game.rgss3a";

        public const string RpgMakerXpProjectFileContent = "RPGXP 1.02";
        public const string RpgMakerVxProjectFileContent = "RPGVX 1.02";
        public const string RpgMakerVxAceProjectFileContent = "RPGVXAce 1.00";

        public const string RpgMakerXpProjectFileExtension = "rxproj";
        public const string RpgMakerVxProjectFileExtension = "rvproj";
        public const string RpgMakerVxAceProjectFileExtension = "rvproj2";

        public static readonly string[] RGSSADHeader = { "RGSSAD\u0000", "Fux2Pac" };

        public const int RGASSDv1 = 1;
        public const int RGASSDv3 = 3;
        public const int RGASSDFux2Pack = 107;

        public static readonly int[] SupportedRGSSVersions = { RGASSDv1, RGASSDv3, RGASSDFux2Pack };

        public static readonly uint RGASSADv1Key = 0xDEADCAFE;

        public const string RPGMakerXpIniFileContents =
            "[Game]\r\nLibrary=RGSS104E.dll\r\nScripts=Data\\Scripts.rxdata\r\nTitle=DecryptedProject\r\nRTP1=Standard\r\nRTP2=\r\nRTP3=";

        public const string RPGMakerVxIniFileContents =
            "[Game]\r\nRTP=RPGVX\r\nLibrary=RGSS202E.dll\r\nScripts=Data\\Scripts.rvdata\r\nTitle=DecryptedProject";

        public const string RPGMakerVxAceIniFileContents =
            "[Game]\r\nRTP=RPGVXAce\r\nLibrary=System\\RGSS300.dll\r\nScripts=Data\\Scripts.rvdata2\r\nTitle=DecryptedProject";
    }
}
