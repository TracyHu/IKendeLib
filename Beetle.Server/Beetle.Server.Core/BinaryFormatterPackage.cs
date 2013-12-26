using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Beetle.Binary
{
    public class Package:Beetle.Protocol.SizePackage
    {
        public Package()
        {
        }

        public Package(Beetle.IChannel channel)
            : base(channel)
        {

        }
        
        protected override Beetle.IMessage ReadMessageByType(Beetle.IDataReader reader, Beetle.ReadObjectInfo typeTag)
        {
            Adapter adapter = new Adapter();
            typeTag.TypeofString = "Adapter";
            return adapter;
        }

        public override object ReadCast(object message)
        {
            return ((Adapter)message).Message;
        }

        public override object WriteCast(object message)
        {
            Adapter adapter = new Adapter();
            adapter.Message = message;
            return adapter;
        }

        protected override void WriteMessageType(Beetle.IMessage msg, Beetle.IDataWriter writer)
        {

        }

        class Adapter : Beetle.IMessage
        {
            public object Message
            {
                get;
                set;
            }

            public void Load(Beetle.IDataReader reader)
            {
                BinaryFormatter bf = new BinaryFormatter();
                Message = bf.Deserialize((Stream)reader);
            }

            public void Save(Beetle.IDataWriter writer)
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize((Stream)writer, Message);
            }
        }

    }
}
