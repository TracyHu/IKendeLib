using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.ComponentModel;
using System.Xml;
using System.Configuration.Install;

namespace Beetle.Server.WinService
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new BeetleWinService() 
			};
            ServiceBase.Run(ServicesToRun);
        }
    }
    [RunInstaller(true)]
    public class WindowsServiceInstaller : Installer
    {

        public WindowsServiceInstaller()
        {

            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.GetDirectoryName(typeof(WindowsServiceInstaller).Assembly.Location) + @"\Beetle.Server.WinService.exe.config");
            XmlNode section = doc.LastChild["beetleServerSection"];

        
            ServiceProcessInstaller serviceProcessInstaller = new ServiceProcessInstaller();
            ServiceInstaller serviceInstaller = new ServiceInstaller();


            serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
            serviceProcessInstaller.Username = null;
            serviceProcessInstaller.Password = null;


            serviceInstaller.DisplayName = section.Attributes["displayName"].Value;
            serviceInstaller.StartType = ServiceStartMode.Automatic;

           
            serviceInstaller.ServiceName = section.Attributes["serverName"].Value;

            this.Installers.Add(serviceProcessInstaller);
            this.Installers.Add(serviceInstaller);
        }
    }
}
