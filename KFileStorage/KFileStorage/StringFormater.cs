using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFileStorage
{
    public class StringFormater:IDataFormater
    {

        public ArraySegment<byte> Write(object data, byte[] recordBuffer)
        {
            string value=(string)data;
            int length = Encoding.UTF8.GetBytes(value, 0, value.Length, recordBuffer, 0);
            return new ArraySegment<byte>(recordBuffer, 0, length);
        }

        public object Read(ArraySegment<byte> data)
        {
            return Encoding.UTF8.GetString(data.Array, 0, data.Count);
        }
    }
}
