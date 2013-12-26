using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ProtoBuf;
using ProtoBuf.Meta;
namespace Beetle.ProtoBuf
{
    public class Package : Beetle.Protocol.SizePackage
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
                if(!type.IsAbstract && !type.IsInterface && type.GetCustomAttributes(typeof(ProtoContractAttribute),false).Length>0)
                    mTables[type.Name]=type;
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
            ProtoAdapter adapter = new ProtoAdapter();
            typeTag.TypeofString = "Adapter";
            return adapter;
        }

        public override object ReadCast(object message)
        {
            return ((ProtoAdapter)message).Message;
        }

        public override object WriteCast(object message)
        {
            ProtoAdapter adapter = new ProtoAdapter();
            adapter.Message = message;
            return adapter;
        }

        protected override void WriteMessageType(Beetle.IMessage msg, Beetle.IDataWriter writer)
        {

        }

        class ProtoAdapter : Beetle.IMessage
        {
            public object Message
            {
                get;
                set;
            }

            public void Load(Beetle.IDataReader reader)
            {
                string type = reader.ReadUTF();
                Message = RuntimeTypeModel.Default.Deserialize((Stream)reader, null, Package.GetType(type));
            }

            public void Save(Beetle.IDataWriter writer)
            {
                writer.WriteUTF(Message.GetType().Name);
                RuntimeTypeModel.Default.Serialize((Stream)writer, Message);
            }
        }


    }
}
