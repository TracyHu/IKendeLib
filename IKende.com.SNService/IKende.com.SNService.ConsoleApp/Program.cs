using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beetle.Express;
using IKende.com.SNService;
namespace IKende.com.SNService.ConsoleApp
{
    class Program
    {
        static ServerFactory mFactory;
        static void Main(string[] args)
        {
            System.Threading.ThreadPool.SetMaxThreads(20, 20);
            System.Threading.ThreadPool.SetMinThreads(20, 20);
            try
            {
                mFactory = new ServerFactory("serverSection");
                foreach (IServer item in mFactory.Servers)
                {
                    Utils.GetLog<Program>().InfoFormat("{0} start @{1}", item.Name, item.Port);
                }
                Utils.GetLog<Program>().InfoFormat("SN Server Started!");
            }
            catch (Exception e_)
            {
                Utils.GetLog<Program>().ErrorFormat("SN Server start error {0}", e_.Message);
            }
            System.Threading.Thread.Sleep(-1);
        }
    }
}
