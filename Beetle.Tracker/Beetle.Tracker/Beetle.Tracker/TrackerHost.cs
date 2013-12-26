using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beetle.Tracker
{
    public class TrackerHost
    {
        public string IPAddress
        {
            get;
            set;
        }
        public int Port
        {
            get;
            set;
        }

        private Beetle.Clients.TcpSyncNode mClient;

        public Beetle.Clients.TcpSyncNode Client
        {
            get
            {
                if (mClient == null)
                {
                    mClient = new Clients.TcpSyncNode(IPAddress, Port, 10);
                    
                }
                return mClient;
            }
        }
    }
}
