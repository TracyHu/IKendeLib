using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beetle.Express.Clients;
namespace IKende.com.SNService.Api
{
    public class SNClient
    {
        public SNClient(string host, int port)
        {

            mPools = new Beetle.Express.Clients.SyncTcpClientPool(host, port);
        }

        private SyncTcpClientPool mPools = null;

        public ulong GetValue()
        {
            TcpReceiveArgs data = mPools.Send("GETSN");
            return BitConverter.ToUInt64(data.Data, 0);
        }
    }
}
