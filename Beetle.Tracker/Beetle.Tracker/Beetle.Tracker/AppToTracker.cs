using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beetle.Tracker
{
    public class AppToTracker<T, P> : IAppToTracker
        where T : IInfoFormater, new()
        where P : IProperties, new()
    {

        public AppToTracker(string appName, params TrackerHost[] hosts)
        {
            Formater = new T();
            mHosts = hosts;
            foreach (TrackerHost item in mHosts)
            {
                item.Client.Connect<Beetle.Clients.TcpSyncChannel<HttpExtend.HttpPacket>>();
            }
            mTrackTime = 1000;
            mTimer = new System.Threading.Timer(OnTarck, null, mTrackTime, mTrackTime);
            mAppName = appName;

        }

        private string mAppName;

        private Dictionary<string, string> mProperties = new Dictionary<string, string>();

        public IDictionary<string, string> Properties
        {
            get
            {
                return mProperties;
            }
        }

        private int mTrackTime;

        private System.Threading.Timer mTimer;

        private TrackerHost[] mHosts;

        [ThreadStatic]
        private static System.IO.Stream mResultStream;

        public System.IO.Stream ResultStream
        {
            get
            {
                if (mResultStream == null)
                    mResultStream = new System.IO.MemoryStream(1024 * 8);
                return mResultStream;
            }

        }

        private void OnTarck(object state)
        {
            mTimer.Change(-1, 0);
            try
            {
                P properties = new P();
                properties.FromHeaders(Properties);
                if (Register != null)
                    Register(this, new EventRegisterArgs { AppToTracker = this, Properties = properties });
                foreach (TrackerHost item in mHosts)
                {
                    try
                    {
                        if (item.Client.Available)
                        {
                            using (Beetle.Clients.TcpSyncNode.Connection connection
                            = item.Client.Pop())
                            {
                                HttpExtend.HttpHeader command = Protocol.Register(mAppName, properties);
                                HttpExtend.HttpHeader result = connection.Send<HttpExtend.HttpHeader>(command);
                                if (result.RequestType != "200")
                                {
                                    Utils.Error<AppToTracker<T, P>>("Register Track {0} Error {1}", item.IPAddress,result.Url);
                                }
                            }
                        }
                    }
                    catch (Exception e__)
                    {
                        Utils.Error<AppToTracker<T, P>>(e__, " Track {0} Error {1}", item.IPAddress, e__.Message);
                    }
                }

                LoadInfo(properties);
            }
            catch (Exception e_)
            {
                Utils.Error<AppToTracker<T, P>>(e_, " Track Error {0}", e_.Message);
            }
            finally
            {
                mTimer.Change(10, mTrackTime);
            }
        }

        private void LoadInfo(IProperties properties)
        {
            foreach (TrackerHost item in mHosts)
            {
                try
                {
                    if (item.Client.Available)
                    {
                        using (Beetle.Clients.TcpSyncNode.Connection connection
                        = item.Client.Pop())
                        {
                           
                            HttpExtend.HttpHeader command = Protocol.GetInfo(mAppName, properties);
                            HttpExtend.HttpHeader result = connection.Send<HttpExtend.HttpHeader>(command);
                            if (result.RequestType == "200" && result.Length > 0)
                            {
                                System.IO.Stream stream = ResultStream;
                                stream.SetLength(0);
                                stream.Position = 0;
                                using (HttpExtend.HttpBody body = connection.Channel.Receive<HttpExtend.HttpBody>())
                                {
                                    stream.Write(body.Data.Array, 0, body.Data.Count);
                                    if (body.Eof)
                                    {
                                        stream.Position = 0;
                                        using (System.IO.StreamReader reader
                                            = new System.IO.StreamReader(stream, Encoding.UTF8))
                                        {
                                            TrackerInfo = Formater.FromString(reader.ReadToEnd());
                                            return;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (result.RequestType == "500")
                                {
                                    Utils.Error<AppToTracker<T, P>>("{2} Get Track {0} info 500 error {1}", item.IPAddress,
                                        result.Url, mAppName);
                                }
                            }
                        }
                    }
                }
                catch (Exception e__)
                {
                    Utils.Error<AppToTracker<T, P>>(e__, "{2} Get Track {0} info Error {1}", item.IPAddress, e__.Message, mAppName);
                }
            }
        }

        public IInfoFormater Formater
        {
            get;
            set;
        }

        public object TrackerInfo
        {
            get;
            set;
        }

        public EventRegister Register
        {
            get;
            set;
        }

    }
}
