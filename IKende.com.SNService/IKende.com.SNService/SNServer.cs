using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IKende.com.SNService
{
    public class SNServer:Beetle.Express.IServerHandler
    {


        public SNServer()
        {
           
        }

        private SNCreater mCreate;


        public void Connect(Beetle.Express.IServer server, Beetle.Express.ChannelConnectEventArgs e)
        {
            Utils.GetLog<SNServer>().InfoFormat("{0} conenct!", e.Channel.EndPoint);
        }

        public void Disposed(Beetle.Express.IServer server, Beetle.Express.ChannelEventArgs e)
        {
            Utils.GetLog<SNServer>().InfoFormat("{0} disposed!", e.Channel.EndPoint);
        }

        public void Error(Beetle.Express.IServer server, Beetle.Express.ErrorEventArgs e)
        {
            Utils.GetLog<SNServer>().InfoFormat("{0} error {1}!", e.Channel==null?"Server":e.Channel.EndPoint.ToString(),e.Error.Message);
        }

        public void Receive(Beetle.Express.IServer server, Beetle.Express.ChannelReceiveEventArgs e)
        {
            if (Encoding.UTF8.GetString(e.Data.Array, e.Data.Offset, e.Data.Count) == "GETSN")
            {
                byte[] sn = mCreate.GetValueData();
                Beetle.Express.Data data = new Beetle.Express.Data();
                data.SetBuffer(sn, 0, 8);
                e.Channel.Server.Send(data, e.Channel);
            }
            else
            {
                e.Channel.Dispose();
            }
        }

        public void SendCompleted(Beetle.Express.IServer server, Beetle.Express.ChannelSendEventArgs e)
        {
           
        }


        public void Opened(Beetle.Express.IServer server)
        {
            mCreate = new SNCreater((uint)SNServiceSection.Instance.Sequence.Start,
                (uint)SNServiceSection.Instance.Sequence.Step);
        }
    }
}
