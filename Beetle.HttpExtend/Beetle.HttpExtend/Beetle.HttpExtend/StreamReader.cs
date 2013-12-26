using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beetle.HttpExtend
{
    public class StreamReader
    {
        public StreamReader(System.IO.Stream stream)
        {
            mStream = stream;
            mReadCount = stream.Length;
        }

        private System.IO.Stream mStream;

        private long mReadCount = 0;

        public bool Read()
        {
            return mReadCount > 0;
        }

        public void ReadTo(HttpBody body)
        {
            long length = mReadCount >= body.Data.BufferLength ? body.Data.BufferLength : mReadCount;
            mStream.Read(body.Data.Array, 0, (int)length);
            body.Data.SetInfo(0, (int)length);
            mReadCount -= length;
            if (mReadCount == 0)
                body.Eof = true;
        }

    }

}
