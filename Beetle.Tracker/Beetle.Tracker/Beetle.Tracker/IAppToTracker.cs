using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beetle.Tracker
{
    public interface IAppToTracker
    {
        IInfoFormater Formater
        {
            get;
            set;
        }

        object TrackerInfo
        {
            get;
            set;
        }

        EventRegister Register
        {
            get;
            set;
        }
        IDictionary<string, string> Properties
        {
            get;
        }
    }
}
