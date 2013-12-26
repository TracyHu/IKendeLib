using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IKende.com.SNService
{
    public class Utils
    {
        static Utils()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        public static log4net.ILog GetLog<T>()
        {
            return log4net.LogManager.GetLogger(typeof(T));
        }
    }
}
