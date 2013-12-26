using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beetle.Server.Console
{
    class Program
    {
        private static Core.ServerFactory mServerFactory;

        static void Main(string[] args)
        {
            try
            {
                System.Console.WindowWidth = 160;
                Core.Utils.GetLog<Program>().Info("Beetle Server Copyright @ henryfan 2013 Version " + typeof(Program).Assembly.GetName().Version);
                Core.Utils.GetLog<Program>().Info("Website:http://www.ikende.com");
                Core.Utils.GetLog<Program>().Info("Email:henryfan@msn.com");
                Beetle.TcpUtils.Setup("beetle");
                Core.Utils.GetLog<Program>().Info(Beetle.LICENSE.GetLICENSE().ToString().Replace("\r\n", "\r\n\t\t\t\t"));
                Core.Utils.GetLog<Program>().Info("beetle installed!");
                mServerFactory = new Core.ServerFactory("beetleServerSection");
                mServerFactory.Open();
                Core.Utils.GetLog<Program>().InfoFormat("beetle win service start at {0}", DateTime.Now);
            }
            catch (Exception e_)
            {
                Core.Utils.GetLog<Program>().ErrorFormat("beetle server start error {0}", e_.Message);
            }
            System.Threading.Thread.Sleep(-1);
        }
    }
}
