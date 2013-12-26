using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beetle.Tracker
{
    class Utils
    {
        static Utils()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        public static log4net.ILog GetLog<T>()
        {
            return log4net.LogManager.GetLogger(typeof(T));
        }
        public static void Error<T>(Exception e)
        {
            GetLog<T>().Error(e.Message, e);
        }
        public static void Error<T>(Exception e,string format,params object[] values)
        {
            GetLog<T>().Error(string.Format(format,values), e);
        }
        public static void Error<T>(string format, params object[] values)
        {
            Error<T>(string.Format(format, values));
        }
        public static void Error<T>(string error)
        {
            GetLog<T>().Error(error);
        }
    }
}
