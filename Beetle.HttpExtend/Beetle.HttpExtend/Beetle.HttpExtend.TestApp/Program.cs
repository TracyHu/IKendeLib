using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beetle.HttpExtend.TestApp
{
    class Program:Beetle.ServerBase<HttpExtend.HttpPacket>
    {

        private static Program mServer = new Program();

        private static int Closeds = 0;

        private static int Connections = 0;

        private static int SendMessages;

        static void Main(string[] args)
        {
            Beetle.TcpUtils.Setup("beetle");
            Console.WriteLine(Beetle.LICENSE.GetLICENSE());
            mServer.Listens = 1000;
            mServer.Open(8089);
            Console.WriteLine("Http Extend Listen @8089");
            while (true)
            {
                System.Threading.Thread.Sleep(5000);
                Console.WriteLine("{0}/{1} send messages:{2}",Closeds,Connections,SendMessages);
            }

        }

        protected override void OnConnected(object sender, ChannelEventArgs e)
        {
            base.OnConnected(sender, e);
            System.Threading.Interlocked.Increment(ref Connections);
          //  Console.WriteLine("{0} connected!", e.Channel.EndPoint);
             e.Channel.EnabledSendCompeletedEvent = true;
             e.Channel.SendMessageCompleted = (o, i) => {
                 if (e.Channel != null && e.Channel["BODY_COMPLETED"] != null)
                 {
                     bool completed = (bool)e.Channel["BODY_COMPLETED"];
                     if (completed)
                     {
                       // e.Channel.Dispose();
                         System.Threading.Interlocked.Increment(ref Closeds);
                         System.Threading.Interlocked.Add(ref SendMessages, i.Messages.Count);
                     }
                 }
                 else
                 {

                 }
             };
        }

        protected override void OnError(object sender, ChannelErrorEventArgs e)
        {
            base.OnError(sender, e);
            Console.WriteLine("{0} error {1}!", e.Channel.EndPoint,e.Exception.Message);
        }

        protected override void OnMessageReceive(PacketRecieveMessagerArgs e)
        {
            base.OnMessageReceive(e);
            if (e.Message is HttpExtend.HttpHeader)
            {
                HttpExtend.HttpHeader header = e.Message as HttpExtend.HttpHeader;
                /*Console.WriteLine(header.Command);
                foreach (string key in header.Properties.Keys)
                {
                    Console.WriteLine("{0}:{1}", key, header[key]);
                }*/

                header = new HttpHeader();
                header.Action = "HTTP/1.1 200 OK";
                header["Cache-Control"] = "private";
                header.ContentType = "text/html; charset=utf-8";
                header.Server = "Beetle Http Extend";
                header.Connection = "keep-alive";
                FileReader reader = new FileReader("h:\\KendeSoft.htm");
               // HttpBody body = HttpPacket.InstanceBodyData();
               // body.Data.Encoding("<p>beetle http extend server!</p>", Encoding.UTF8);
               // body.Eof = true;
               // header.Length = body.Data.Count;
                header.Length = reader.FileInfo.Length;
                e.Channel.Send(header);
               // e.Channel.Send(body);
                while (reader.Read())
                {
                    HttpBody body = HttpPacket.InstanceBodyData();
                    reader.ReadTo(body);
                    e.Channel.Send(body);
                }

                
            }
        }

        protected override void OnDisposed(object sender, ChannelDisposedEventArgs e)
        {
            base.OnDisposed(sender, e);
           // Console.WriteLine("{0} disposed!", e.Channel.EndPoint);
        }
    }

}
