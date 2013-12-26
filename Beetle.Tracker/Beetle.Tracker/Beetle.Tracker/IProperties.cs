using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beetle.Tracker
{
    public interface IProperties
    {
        void FromHeaders(IDictionary<string, string> header);
        IDictionary<string, string> ToHeaders();
    }
    public class Properties : IProperties
    {

        public Properties()
        {
        }
      
        private IDictionary<string, string> mProperties = new Dictionary<string, string>();

        public string this[string name]
        {
            get
            {
                return mProperties[name];
            }
            set
            {
                mProperties[name] = value;
            }
        }

        public void FromHeaders(IDictionary<string, string> header)
        {
            foreach (string key in header.Keys)
            {
                mProperties[key] = header[key];
            }
        }

        public IDictionary<string, string> ToHeaders()
        {
            return this.mProperties;
        }
    }
}
