using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beetle.Json
{
    public class Package: Beetle.Protocol.SizePackage
    {
        public Package()
        {
        }
        public Package(Beetle.IChannel channel)
            : base(channel)
        {

        }

        public static void Register(System.Reflection.Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if(!type.IsAbstract && !type.IsInterface)
                    mTables[type.FullName]=type;
            }
        }

        static System.Collections.Hashtable mTables = new System.Collections.Hashtable(256);

        public static Type GetType(string name)
        {
            Type type = null;
            type = (Type)mTables[name];
            if (type == null)
                type = Type.GetType(name);
            return type;
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
                string type = reader.ReadUTF();
                string data = reader.ReadString((int)(reader.Length-reader.Position));
                Message = Newtonsoft.Json.JsonConvert.DeserializeObject(data, Package.GetType(type));
            }

            public void Save(Beetle.IDataWriter writer)
            {
                writer.WriteUTF(Message.GetType().FullName);
                writer.WriteString(Newtonsoft.Json.JsonConvert.SerializeObject(Message));
            }
        }


    }
}
