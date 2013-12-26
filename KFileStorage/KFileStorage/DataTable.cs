using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFileStorage
{
    class DataTable:IDisposable
    {
        public DataTable(string filename, int size,int count)
        {
            mRecordSize = size;
            mCount = count;
            if (!System.IO.File.Exists(filename))
            {
                CreateDataFile(filename, size, count);
            }
            mWriteStream = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Write, System.IO.FileShare.ReadWrite);
            mReadStream = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
        }

        private System.IO.Stream mWriteStream;

        private System.IO.Stream mReadStream;

        private int mCount;

        private int mRecordSize;

        private void CreateDataFile(string name, int size, int count)
        {
            byte[] buffer = new byte[size+4];
            using(System.IO.FileStream stream = System.IO.File.Open(name, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            {
                for (int i = 0; i < count; i++)
                {
                    stream.Write(buffer, 0, buffer.Length);
                }
            }
        }

        public void Set(ArraySegment<byte> data, int index)
        {
            lock (mWriteStream)
            {
                DataRecord dr = new DataRecord(index, mRecordSize);
                dr.SetData(data, mWriteStream);
            }
        }

        public ArraySegment<byte> Get(int index, byte[] buffer)
        {
            lock (mReadStream)
            {
                DataRecord dr = new DataRecord(index, mRecordSize);
                return dr.GetData(mReadStream, buffer);
            }
        }

        public void Dispose()
        {
            mWriteStream.Dispose();
            mReadStream.Dispose();
        }
    }
}
