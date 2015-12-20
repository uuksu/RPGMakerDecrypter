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
            RGSSAD rgssad = new RGSSAD("Game.rgssad");
            Console.WriteLine(rgssad.GetVersion());
            Console.ReadKey();
        }
    }
}
