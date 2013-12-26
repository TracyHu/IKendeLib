using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beetle.Tracker
{
    public  delegate void EventRegister(object sender,EventRegisterArgs e);

    public class EventRegisterArgs:EventArgs
    {
        public IProperties Properties
        {
            get;
            set;
        }
        public IAppToTracker AppToTracker
        {
            get;
            set;
        }
    }
}
