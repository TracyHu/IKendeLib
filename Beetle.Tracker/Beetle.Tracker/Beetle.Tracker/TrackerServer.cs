using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beetle.Tracker
{
    public class TrackerServer:Beetle.IServerHandler
    {
        public TrackerServer()
        {
            if (TrackerServerSection.Instance != null)
            {
                foreach (AppTrackerConfig conf in TrackerServerSection.Instance.Trackers)
                {
                    Type type = Type.GetType(conf.Type);
                    if (type != null)
                    {
                        mAppHandlers[conf.Name] = (IAppTrackerHandler)Activator.CreateInstance(type);
                    }
                    else
                    {
                        Utils.GetLog<TrackerServer>().ErrorFormat("{0} AppTrackerHandler Notfound!", conf.Type);
                    }
                }
            }
        }

        public void AddApp<T>(string name) where T:IAppTrackerHandler,new()
        {
            mAppHandlers[name] = new T();
        }

        private Dictionary<string, IAppTrackerHandler> mAppHandlers = new Dictionary<string, IAppTrackerHandler>();

        public IAppTrackerHandler GetTackerHandler(string appname)
        {
            IAppTrackerHandler handler = null;
            mAppHandlers.TryGetValue(appname, out handler);
            return handler;
        }

        public void ChannelCreated(ServerBase server, ChannelEventArgs e)
        {
            Utils.GetLog<TrackerServer>().InfoFormat("{0} Connected TrackerServer", e.Channel.EndPoint);
        }

        public void ChannelDisconnect(ServerBase server, ChannelDisposedEventArgs e)
        {
            Utils.GetLog<TrackerServer>().InfoFormat("{0} Discontected TrackerServer", e.Channel.EndPoint);
        }

        public void ChannelError(ServerBase server, ChannelErrorEventArgs e)
        {
            Utils.Error<TrackerServer>(e.Exception, "TrackerServer Channel {1} Error {0}", e.Exception.Message,e.Channel.EndPoint);
        }

        public void ChannelReceiveMessage(ServerBase server, PacketRecieveMessagerArgs e)
        {
            try
            {
                HttpExtend.HttpHeader hader = (HttpExtend.HttpHeader)e.Message;
                Properties ps = new Properties();
                ps.FromHeaders(hader.Properties);
                switch (hader.RequestType)
                {
                    case Protocol.COMMAND_GET:
                        OnGet(e.Channel, hader.Url, ps);
                        break;
                    case Protocol.COMMAND_GETINFO:
                        OnGetInfo(e.Channel, hader.Url, ps);
                        break;
                    case Protocol.COMMAND_REGISTER:
                        OnRegister(e.Channel, hader.Url, ps);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e_)
            {
                HttpExtend.HttpHeader header = Protocol.GetResponse(new Properties());
                header.Action = "500 " + e_.Message;
                e.Channel.Send(header);
            }
        }


        private void OnRegister(Beetle.IChannel channel, string appname, IProperties properties)
        {

        }

        private void OnGetInfo(Beetle.IChannel channel, string appname, IProperties properties)
        {

        }

        private void OnGet(Beetle.IChannel channel, string appname, IProperties properties)
        {

        }


        public void Disposed(ServerBase server)
        {
            Utils.GetLog<TrackerServer>().Info("TrackerServer Disposed!");
        }

        public void Opened(ServerBase server)
        {
            Utils.GetLog<TrackerServer>().InfoFormat("TrackerServer Start @{0}", server.Server.Socket.LocalEndPoint);
        }

        public void ServerError(ServerBase server, TcpServerErrorArgs e)
        {
            Utils.Error<TrackerServer>(e.Error, "TrackerServer Error {0}", e.Error.Message);
        }

        public void SocketConnect(ServerBase server, ChannelCreatingArgs e)
        {
           
        }
    }
}
