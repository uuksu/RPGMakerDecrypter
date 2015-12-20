using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGMakerDecrypter.Decrypter
{
    public class ArchivedFile
    {
        public string Name { get; set; }

        public int Size { get; set; }

        public long Offset { get; set; }

        public uint Key { get; set; }
    }
}
