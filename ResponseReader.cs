using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigBang1112.TmXmlRpc
{
    public class ResponseReader : BinaryReader
    {
        public ResponseReader(Stream input) : base(input)
        {

        }

        public override string ReadString()
        {
            return ReadString(ReadInt32());
        }

        public string ReadString(int length)
        {
            return Encoding.UTF8.GetString(ReadBytes(length));
        }

        public int[] ReadArrayInt32(int length)
        {
            var array = new int[length];

            for (var i = 0; i < length; i++)
                array[i] = ReadInt32();

            return array;
        }

        public int[] ReadArrayInt32()
        {
            return ReadArrayInt32(ReadInt32());
        }

        public byte[] ReadArrayByte(int length)
        {
            var array = new byte[length];

            for (var i = 0; i < length; i++)
                array[i] = ReadByte();

            return array;
        }

        public byte[] ReadArrayByte()
        {
            return ReadArrayByte(ReadInt32());
        }

        public string[] ReadArrayString(int length)
        {
            var array = new string[length];

            for (var i = 0; i < length; i++)
                array[i] = ReadString();

            return array;
        }

        public string[] ReadArrayString()
        {
            return ReadArrayString(ReadInt32());
        }
    }
}
