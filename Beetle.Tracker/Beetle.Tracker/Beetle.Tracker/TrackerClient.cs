using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beetle.Tracker
{
    public class TrackerClient
    {
        public TrackerClient(string host, int port)
        {
            mNode = new Clients.TcpSyncNode(host, port, 10);
            mNode.Connect<Beetle.Clients.TcpSyncChannel<HttpExtend.HttpPacket>>();
        }

        private Beetle.Clients.TcpSyncNode mNode;

        
    }
}
