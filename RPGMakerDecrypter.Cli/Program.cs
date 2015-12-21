using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPGMakerDecrypter.Decrypter;

namespace RPGMakerDecrypter.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            //RGSSADv1 rgssad = new RGSSADv1(Constants.RpgMakerXpArchiveName);
            //rgssad.ExtractAllFiles("Output", true);
            //Console.ReadKey();

            RGSSADv3 rgssadv3 = new RGSSADv3(Constants.RpgMakerVxAceArchiveName);
            rgssadv3.ExtractAllFiles("Output", true);
        }
    }
}
