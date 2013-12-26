using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace Beetle.Server.WinService
{
    public partial class BeetleWinService : ServiceBase
    {
        public BeetleWinService()
        {
            InitializeComponent();
        }

        private Core.ServerFactory mServerFactory;

        protected override void OnStart(string[] args)
        {
            try
            {
                Core.Utils.GetLog<BeetleWinService>().Info("Beetle Server Copyright @ henryfan 2013 Version " + typeof(BeetleWinService).Assembly.GetName().Version);
                Core.Utils.GetLog<BeetleWinService>().Info("Website:http://www.ikende.com");
                Core.Utils.GetLog<BeetleWinService>().Info("Email:henryfan@msn.com");
                Beetle.TcpUtils.Setup("beetle");
                Core.Utils.GetLog<BeetleWinService>().Info(Beetle.LICENSE.GetLICENSE().ToString().Replace("\r\n", "\r\n\t\t\t\t"));
                Core.Utils.GetLog<BeetleWinService>().Info("beetle installed!");
                mServerFactory = new Core.ServerFactory("beetleServerSection");
                mServerFactory.Open();
                Core.Utils.GetLog<BeetleWinService>().InfoFormat("beetle win service start at {0}",DateTime.Now);
            }
            catch (Exception e_)
            {
                Core.Utils.GetLog<BeetleWinService>().ErrorFormat("beetle server start error {0}", e_.Message);
            }
        }

        protected override void OnStop()
        {
            try
            {
               
                Beetle.TcpUtils.Setup("beetle");
                try
                {
                    mServerFactory.Stop();
                }
                catch (Exception e)
                {
                    Core.Utils.GetLog<BeetleWinService>().ErrorFormat("beetle server stop error {0}", e.Message);
                }
              
                Beetle.TcpUtils.Clean();
                Core.Utils.GetLog<BeetleWinService>().InfoFormat("beetle win service stop at {0}", DateTime.Now);
            }
            catch (Exception e_)
            {
                Core.Utils.GetLog<BeetleWinService>().ErrorFormat("beetle server stop error {0}", e_.Message);
            }
        }
    }
}
