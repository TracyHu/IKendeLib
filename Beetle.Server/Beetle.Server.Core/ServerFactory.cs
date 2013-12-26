using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beetle.Server.Core
{
    public class ServerFactory
    {
        public ServerFactory(string section)
        {
            mSection = (BeetleServerSection)System.Configuration.ConfigurationManager.GetSection(section);
            if (mSection == null)
            {
                Utils.GetLog<ServerFactory>().ErrorFormat("{0} section not found", section);

            }
            else
            {
                foreach (BeetleServerConf conf in mSection.Servers)
                {
                    mServers.Add(new ServerItem(conf));
                }
            }
        }

        private System.Threading.Timer mStaticTimer;

        private BeetleServerSection mSection;

        private IList<ServerItem> mServers = new List<ServerItem>();

      

        private void BeetleStatic(object state)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Beetle Status\r\n");
            sb.AppendFormat("\t\t\t\tReceive SAEA   Pool:{0}\r\n", TcpUtils.GetAsyncEventPoolStatus());
            sb.AppendFormat("\t\t\t\tReceive Buffer Pool:{0}\r\n", TcpUtils.GetReceiveBufferStatus());
            sb.AppendFormat("\t\t\t\tSend    Buffer Pool:{0}\r\n", TcpUtils.GetSendBufferStatus());
            sb.AppendFormat("\t\t\t\tReceive Bytes      :{0}\r\n", TcpUtils.ReceiveBytes);
            sb.AppendFormat("\t\t\t\tSend    Bytes      :{0}\r\n", TcpUtils.SendBytes);
            sb.AppendFormat("\t\t\t\tReceive Messages   :{0}\r\n", TcpUtils.ReceiveMessages);
            sb.AppendFormat("\t\t\t\tSend    Messages   :{0}\r\n", TcpUtils.SendMessages);
            IList<int> receivequeue = TcpUtils.GetReceiveDespatchStatus();
            IList<int> sendqueue = TcpUtils.GetSendDespatchStatus();
            IList<int> workqueue = TcpUtils.GetWorkDespatchsStatus();
            sb.AppendFormat("\t\t\t\tReceive       Queue:");
            for(int i =0;i<receivequeue.Count;i++)
            {
                sb.AppendFormat("[D{0}:{1}]",i, receivequeue[i]);
            }
            sb.AppendFormat("\r\n");

            sb.AppendFormat("\t\t\t\tSend          Queue:");
            for (int i = 0; i < sendqueue.Count; i++)
            {
                sb.AppendFormat("[D{0}:{1}]", i, sendqueue[i]);
            }
            sb.AppendFormat("\r\n");

            sb.AppendFormat("\t\t\t\tWork          Queue:");
            for (int i = 0; i < workqueue.Count; i++)
            {
                sb.AppendFormat("[D{0}:{1}]", i, workqueue[i]);
            }
            sb.AppendFormat("\r\n");
            foreach (ServerItem item in mServers)
            {
                if (item.Server != null)
                {
                    sb.AppendFormat("\t\t\t\t{0} [listen:{1}] [Online:{2}]\r\n", item.Name,item.Server.Server.Socket.LocalEndPoint, item.Server.Server.Count);
                }
            }
            Utils.GetLog<ServerFactory>().Info(sb.ToString());
        }

        public void Open()
        {
            foreach (ServerItem item in mServers)
            {
                if (item.Installed)
                {
                    try
                    {

                        if (string.IsNullOrEmpty(item.Host))
                        {
                            item.Server.Open(item.Port);
                        }
                        else
                        {
                            item.Server.Open(item.Host, item.Port);
                        }
                        Utils.GetLog<ServerFactory>().InfoFormat("{0} start completed", item.Name);
                    }
                    catch (Exception e_)
                    {
                        Utils.GetLog<ServerFactory>().ErrorFormat("{0} start error {1}", item.Name, e_.Message);
                    }
                }
            }
            if(Beetle.TcpUtils.Statistics)
                mStaticTimer = new System.Threading.Timer(BeetleStatic, null, 5000, 5000);
            
        }
        public void Stop()
        {
            if (mStaticTimer != null)
                mStaticTimer.Dispose();
            foreach (ServerItem item in mServers)
            {
                if (item.Installed)
                {
                    item.Server.Dispose();
                }
            }
        }
    }
}
