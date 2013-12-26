using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beetle.Server.TestImpl
{
    public class TestServer:Beetle.IServerHandler
    {
        public log4net.ILog Log
        {
            get
            {
                return log4net.LogManager.GetLogger(typeof(TestServer));
            }
        }

        public void ChannelCreated(ServerBase server, ChannelEventArgs e)
        {
            Log.InfoFormat("create channel@{0}", e.Channel.EndPoint);
        }

        public void ChannelDisconnect(ServerBase server, ChannelDisposedEventArgs e)
        {
            Log.WarnFormat("{0} disconnected!", e.Channel.EndPoint);
        }

        public void ChannelError(ServerBase server, ChannelErrorEventArgs e)
        {
            if (e.Exception.InnerException != null)
            {
                Log.ErrorFormat("{0} channel error {1} inner error {2}", e.Channel.EndPoint, e.Exception.Message, e.Exception.InnerException.Message);
            }
            else
            {
                Log.ErrorFormat("{0} channel error {1} ", e.Channel.EndPoint, e.Exception.Message);
            }
        }

        public void ChannelReceiveMessage(ServerBase server, PacketRecieveMessagerArgs e)
        {
            
        }

        public void Disposed(ServerBase server)
        {
            Log.WarnFormat("{0} server disposed!", server.Name);
        }

        public void Opened(ServerBase server)
        {
            Log.InfoFormat("{0} server open @{1}", server.Name, server.Server.Socket.LocalEndPoint);
        }

        public void ServerError(ServerBase server, TcpServerErrorArgs e)
        {
            
        }

        public void SocketConnect(ServerBase server, ChannelCreatingArgs e)
        {
            Log.InfoFormat("accept connection @{0}", e.Socket.RemoteEndPoint);
        }
    }
}
