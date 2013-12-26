using System;
using System.Text;
using System.Threading;
using Beetle;
namespace Beetle.HttpExtend
{
    public class HttpPacket : Package
    {

     

        public const int BODY_BLOCK_SIZE = 1024 * 16;

        public static ByteArrayPool BodyBufferPool = new ByteArrayPool(1024, BODY_BLOCK_SIZE);

        private HttpHeader mHeader = null;

        private long? mContentLength = null;

        private byte[] mHeaderBuffer = HeaderBufferPool.Instance.Pop();

        private PacketRecieveMessagerArgs mReceiveMessage = new PacketRecieveMessagerArgs(null, null);

        public static HttpBody InstanceBodyData()
        {
            HttpBody body = new HttpBody();
            body.Data = BodyBufferPool.Pop();
            return body;
          
        }

        public HttpPacket()
        {

        }

        public HttpPacket(IChannel channel)
            : base(channel)
        {
        }

        public override void Import(byte[] data, int start, int count)
        {
            while (count > 0)
            {
                if (mContentLength == null)
                {
                    if (mHeader == null)
                    {
                        mHeader = new HttpHeader(mHeaderBuffer);
                    }
                    if (mHeader.Import(data, ref start, ref count))
                    {
                        if (mHeader.Length == 0)
                        {
                            mReceiveMessage.Channel = this.Channel;
                            mReceiveMessage.Message = mHeader;
                            mHeader = null;
                            mContentLength = null;
                            OnReceiveMessage(mReceiveMessage);
                               
                        }
                        else
                        {
                            mContentLength = mHeader.Length;
                        }
                    }
                }
                else
                {
                    HttpBody body = InstanceBodyData();
                    int blockSize = (int)(mContentLength > BODY_BLOCK_SIZE ? BODY_BLOCK_SIZE : mContentLength);
                    blockSize = blockSize > count ? count : blockSize;
                    ByteArraySegment segment = body.Data;
                    Buffer.BlockCopy(data, start, segment.Array, 0, blockSize);
                    count -= blockSize;
                    mContentLength -= blockSize;
                    body.Eof = mContentLength == 0;
                    if (body.Eof)
                    {
                        mContentLength = null;
                        mHeader = null;
                    }
                    mReceiveMessage.Channel = this.Channel;
                    mReceiveMessage.Message = body;
                    OnReceiveMessage(mReceiveMessage);
                }
            }

        }

        protected override void OnDisposed()
        {
            if (mHeaderBuffer != null)
            {
                HeaderBufferPool.Instance.Push(mHeaderBuffer);
                mHeaderBuffer = null;
            }
        }

        public override IMessage MessageRead(IDataReader reader)
        {
            return null;
        }

        public override void MessageWrite(IMessage msg, IDataWriter writer)
        {
            msg.Save(writer);
            if (msg is HttpBody)
            {
                ((HttpBody)msg).Dispose();
            }
        }
    }
}
