using RPGMakerDecrypter.Decrypter.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RPGMakerDecrypter.Decrypter
{
    public class RGSSADv3Fux2Pack : RGSSADv3
    {
        public RGSSADv3Fux2Pack(BinaryReader inBinaryReader) : base(inBinaryReader)
        {
        }
        protected override void PrepareKey()
        {
            key = (uint)BinaryReader.ReadInt32();
        }
    }
}
