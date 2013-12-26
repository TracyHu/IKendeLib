using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFileStorage
{
    class DataRecord
    {
        public DataRecord(int index, int size)
        {
            Index = index;
            Size = size;
        }
        public int Index
        {
            get;
            set;
        }
        public int Size
        {
            get;
            set;
        }

        private void Seek(System.IO.Stream stream)
        {
            stream.Position = Index * Size + 4;
        }

        public void SetData(ArraySegment<byte> data,System.IO.Stream stream)
        {
            Seek(stream);
            stream.Write(BitConverter.GetBytes(data.Count),0,4);
            stream.Write(data.Array, data.Offset, data.Count);
            stream.Flush();
        }
        public ArraySegment<byte> GetData(System.IO.Stream stream,byte[] buffer)
        {
            Seek(stream);
            byte[] lendata = new byte[4];
            stream.Read(lendata, 0, 4);
            int length = BitConverter.ToInt32(lendata,0);
            if (length > 0)
            {
                stream.Read(buffer, 0, length);
            }
            return new ArraySegment<byte>(buffer, 0, length);
        }
    }
}
