using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beetle.HttpExtend
{
    public class HeaderBufferPool
    {

        private int mBufferSize = HttpHeader.HEADER_BUFFER_LENGT;

        public HeaderBufferPool(int size = HttpHeader.HEADER_BUFFER_LENGT)
        {
            mBufferSize = size;
            for (int i = 0; i < 1000; i++)
            {
                mPool.Push(new byte[mBufferSize]);
            }
        }

        private System.Collections.Concurrent.ConcurrentStack<byte[]> mPool = new System.Collections.Concurrent.ConcurrentStack<byte[]>();

        public byte[] Pop()
        {
            byte[] result = null;
            if (mPool.TryPop(out result))
                return result;
            return new byte[mBufferSize];
        }

        public void Push(byte[] data)
        {
            mPool.Push(data);
        }

        private static HeaderBufferPool mInstance = new HeaderBufferPool();

        public static HeaderBufferPool Instance
        {
            get
            {
                return mInstance;
            }
        }
    }
}
