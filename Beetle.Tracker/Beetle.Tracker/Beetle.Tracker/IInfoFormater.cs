using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beetle.Tracker
{
    public interface IInfoFormater
    {
        object FromString(string value);
        string ToStringValue(object obj);
    }
}
