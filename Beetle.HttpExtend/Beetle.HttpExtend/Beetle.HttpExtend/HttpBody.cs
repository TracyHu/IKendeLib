using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beetle.HttpExtend
{
    public class HttpBody : IMessage, IDisposable
    {
        internal HttpBody()
        {
        }
        public bool Eof
        {
            get;
            set;
        }
        public ByteArraySegment Data
        {
            get;
            set;
        }


        public void Load(IDataReader reader)
        {

        }

        public void Save(IDataWriter writer)
        {
            writer.Write(Data.Array, Data.Offset, Data.Count);
            writer.Channel["BODY_COMPLETED"] = Eof;
        }

        private int mIsDisposed = 0;

        public void Dispose()
        {

           lock(this)
           {
                if (Data != null)
                {
                    HttpPacket.BodyBufferPool.Push(Data);
                    Data = null;
                }
            }

        }
    }
}
