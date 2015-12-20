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
            RGSSADv1 rgssad = new RGSSADv1("Game.rgssad");
            Console.ReadKey();
        }
    }
}
