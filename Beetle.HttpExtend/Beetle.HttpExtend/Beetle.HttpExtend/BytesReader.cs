using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beetle.HttpExtend
{
    public class BytesReader
    {
        public BytesReader(byte[] data, int postion, int length)
        {
            mData = data;
            mPostion = postion;
            mReadCount = length;
        }

        private byte[] mData;

        private long mPostion;

        private long mReadCount = 0;

        public bool Read()
        {
            return mReadCount > 0;
        }

        public void ReadTo(HttpBody body)
        {
            long length = mReadCount >= body.Data.BufferLength ? body.Data.BufferLength : mReadCount;
            Buffer.BlockCopy(mData, (int)mPostion, body.Data.Array, 0, (int)length);
            
            body.Data.SetInfo(0, (int)length);
            mReadCount -= length;
            mPostion += length;
            if (mReadCount == 0)
                body.Eof = true;
        }
    }
}
