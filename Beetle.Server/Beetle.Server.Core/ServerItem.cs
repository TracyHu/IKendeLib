using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beetle.Server.Core
{
    public class ServerItem
    {
        public ServerItem(BeetleServerConf conf)
        {
            Type packateType = Type.GetType(conf.Package);
            if (packateType == null)
            {
                Utils.GetLog<ServerItem>().ErrorFormat("{0} server instance [{1}] package type not found!",conf.Name, conf.Package);
                return;
            }
            try
            {
                Type servertype = Type.GetType("Beetle.ServerImpl`1,Beetle");
                Type impltype = servertype.MakeGenericType(packateType);
                Type handlertype = Type.GetType(conf.Handler);
                if(handlertype ==null)
                    Utils.GetLog<ServerItem>().ErrorFormat("{0} server instance [{1}] handler type not found!",conf.Name, conf.Handler);
                Server = (ServerBase)Activator.CreateInstance(impltype,conf.Name,Activator.CreateInstance(handlertype));
            }
            catch (Exception e_)
            {
                Utils.GetLog<ServerItem>().ErrorFormat("{0} sever instance error {1}", conf.Name, e_.Message);
            }
            Host = conf.Host;
            Port = conf.Port;
            Name = conf.Name;
            Installed = true;
        }
        public bool Installed
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public ServerBase Server
        {
            get;
            set;
        }
        public string Host
        {
            get;
            set;
        }
        public int Port
        {
            get;
            set;
        }
    }
}
