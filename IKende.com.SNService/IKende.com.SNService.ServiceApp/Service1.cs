using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Beetle.Express;
using System.Configuration.Install;

namespace IKende.com.SNService.ServiceApp
{
    public partial class SNServiceApp : ServiceBase
    {
        public SNServiceApp()
        {
            InitializeComponent();
        }

        private ServerFactory mFactory;

        protected override void OnStart(string[] args)
        {
            try
            {
                mFactory = new ServerFactory("serverSection");
                foreach (IServer item in mFactory.Servers)
                {
                    Utils.GetLog<SNServiceApp>().InfoFormat("{0} start @{1}", item.Name, item.Port);
                }
                Utils.GetLog<SNServiceApp>().InfoFormat("SN Server Started!");
            }
            catch (Exception e_)
            {
                Utils.GetLog<SNServiceApp>().ErrorFormat("SN Server start error {0}", e_.Message);
            }
        }

        protected override void OnStop()
        {
            if (mFactory != null)
            {
                foreach (IServer item in mFactory.Servers)
                {
                    item.Dispose();
                }
            }
        }
    }

    [RunInstaller(true)]
    public class WindowsServiceInstaller : Installer
    {
        /// <summary>
        /// Public Constructor for WindowsServiceInstaller.
        /// - Put all of your Initialization code here.
        /// </summary>
        public WindowsServiceInstaller()
        {
            ServiceProcessInstaller serviceProcessInstaller =
                               new ServiceProcessInstaller();
            ServiceInstaller serviceInstaller = new ServiceInstaller();

            //# Service Account Information
            serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
            serviceProcessInstaller.Username = null;
            serviceProcessInstaller.Password = null;

            //# Service Information
            serviceInstaller.DisplayName = "IKende SN Service";
            serviceInstaller.StartType = ServiceStartMode.Automatic;

            //# This must be identical to the WindowsService.ServiceBase name
            //# set in the constructor of WindowsService.cs
            serviceInstaller.ServiceName = "IKende SN Service";
            serviceInstaller.Description = "用于分布式唯一ID生成";
            this.Installers.Add(serviceProcessInstaller);
            this.Installers.Add(serviceInstaller);
        }
    }
}
