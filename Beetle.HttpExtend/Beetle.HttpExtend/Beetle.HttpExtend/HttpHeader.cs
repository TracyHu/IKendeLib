using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beetle.HttpExtend
{
    public class HttpHeader : IMessage
    {

        public const int HEADER_BUFFER_LENGT = 1024 * 4;

        public const string HEADER_CONTENT_LENGTH = "Content-Length";

        public const string HEADER_ACCEPT = "Accept";

        public const string HEADER_ACCEPT_ENCODING = "Accept-Encoding";

        public const string HEADER_ACCEPT_LANGUAGE = "Accept-Language";

        public const string HEADER_CONNNECTION = "Connection";

        public const string HEADER_COOKIE = "Cookie";

        public const string HEADER_HOST = "Host";

        public const string HEADER_USER_AGENT = "User-Agent";

        public const string HEADER_CONTENT_ENCODING = "Content-Encoding";

        public const string HEADER_CONTENT_TYPE="Content-Type";

        public const string HEADER_SERVER = "Server";

        private static byte[] mNameEof = Encoding.UTF8.GetBytes(":");

        private static byte[] mWrap = Encoding.UTF8.GetBytes("\r\n");

        public HttpHeader()
        {

        }

        public HttpHeader(byte[] buffer)
        {
            mBuffer = buffer;
        }

        private byte[] mBuffer;

        private int mHeaderLength = 0;

        private int mStartIndex = 0;

        private string mRequestType;

        private string mUrl;

        private string mAction;

        private long? mLength;

        private string mHttpVersion;

        private string mLastPropertyName = null;

        private IDictionary<string, string> mProperties = new Dictionary<string, string>();

        public string Accept
        {
            get
            {
                return this[HEADER_ACCEPT];
            }
            set
            {
                this[HEADER_ACCEPT] = value;
            }
        }

        public string AcceptEncoding
        {
            get
            {
                return this[HEADER_ACCEPT_ENCODING];
            }
            set
            {
                this[HEADER_ACCEPT_ENCODING] = value;
            }
        }

        public string AcceptLanguage
        {
            get
            {
                return this[HEADER_ACCEPT_LANGUAGE];
            }
            set
            {
                this[HEADER_ACCEPT_LANGUAGE] = value;
            }
        }

        public string Connection
        {
            get
            {
                return this[HEADER_CONNNECTION];
            }
            set
            {
                this[HEADER_CONNNECTION] = value;
            }
        }

        public string Cookie
        {
            get
            {
                return this[HEADER_COOKIE];
            }
            set
            {
                this[HEADER_COOKIE] = value;
            }
        }

        public string Host
        {
            get
            {
                return this[HEADER_HOST];
            }
            set
            {
                this[HEADER_HOST] = value;
            }
        }

        public string UserAgent
        {
            get
            {
                return this[HEADER_USER_AGENT];
            }
            set
            {
                this[HEADER_USER_AGENT] = value;
            }
        }

        public string ContentEncoding
        {
            get
            {
                return this[HEADER_CONTENT_ENCODING];
            }
            set
            {
                this[HEADER_CONTENT_ENCODING] = value;
            }
        }

        public string ContentType
        {
            get
            {
                return this[HEADER_CONTENT_TYPE];
            }
            set
            {
                this[HEADER_CONTENT_TYPE] = value;
            }
        }

        public string Server
        {
            get
            {
                return this[HEADER_SERVER];
            }
            set
            {
                this[HEADER_SERVER] = value;
            }
        }

        public string Action
        {
            get
            {
                return mAction;
            }
            set
            {
                mAction = value;
                string[] values = mAction.Split(' ');
                if (values.Length > 0)
                    mRequestType = values[0];
                if (values.Length > 1)
                    mUrl = values[1];
                if (values.Length > 2)
                    mHttpVersion = values[2];

            }
        }

        public string HttpVersion
        {
            get
            {
                return mHttpVersion;
            }
        }

        public string RequestType
        {
            get
            {
                return mRequestType;
            }
        }

        public string Url
        {
            get
            {
                return mUrl;
            }
        }

        public IDictionary<string, string> Properties
        {
            get
            {
                return mProperties;
            }
            set
            {
                mProperties = value;
            }
        }

        public string this[string header]
        {
            get
            {
                string value = null;
                mProperties.TryGetValue(header, out value);
                return value;
            }
            set
            {
                mProperties[header] = value;
            }

        }

        public long Length
        {
            get
            {
                if (mLength == null)
                {
                    string value = this[HEADER_CONTENT_LENGTH];
                    if (value == null)
                        mLength = 0;
                    else
                        mLength = long.Parse(value);
                }
                return mLength.Value;
            }
            set
            {
                mProperties[HEADER_CONTENT_LENGTH] = value.ToString();
            }
        }

        public bool Import(byte[] data, ref int offset, ref int count)
        {
            byte[] buffer = mBuffer;
            while (count > 0)
            {
                buffer[mHeaderLength] = data[offset];
                mHeaderLength++;
                offset++;
                count--;
                if (mHeaderLength >= HEADER_BUFFER_LENGT)
                    throw new NetTcpException("header data too long!");
                if (mBuffer[mHeaderLength - 1] == mWrap[1] && mBuffer[mHeaderLength - 2] == mWrap[0])
                {
                    if (Action == null)
                    {
                        Action = Encoding.UTF8.GetString(buffer, mStartIndex, mHeaderLength - mStartIndex - 2);
                        mStartIndex = mHeaderLength;
                    }
                    else
                    {
                        if (mBuffer[mHeaderLength - 3] == mWrap[1] && mBuffer[mHeaderLength - 4] == mWrap[0])
                        {
                            if (mLastPropertyName != null)
                            {
                                this[mLastPropertyName] = Encoding.UTF8.GetString(buffer, mStartIndex, mHeaderLength - mStartIndex - 2);
                            }
                            return true;
                        }
                        else
                        {
                            if (mLastPropertyName != null)
                            {
                                this[mLastPropertyName] = Encoding.UTF8.GetString(buffer, mStartIndex, mHeaderLength - mStartIndex - 2);
                                mStartIndex = mHeaderLength;
                                mLastPropertyName = null;
                            }
                        }
                    }
                }
                else if (mBuffer[mHeaderLength - 1] == mNameEof[0] && mLastPropertyName == null)
                {
                    mLastPropertyName = Encoding.UTF8.GetString(buffer, mStartIndex, mHeaderLength - mStartIndex - 1);
                    mStartIndex = mHeaderLength;
                }

            }
            return false;

        }

        public void Load(IDataReader reader)
        {

        }

        public void Save(IDataWriter writer)
        {
            writer.WriteString(Action + "\r\n");
            foreach (string key in mProperties.Keys)
            {
                writer.WriteString(string.Format("{0}: {1}\r\n", key, mProperties[key]));
            }
            writer.WriteString("\r\n");


        }

        public ArraySegment<byte> Export(byte[] buffer)
        {
            System.IO.MemoryStream stream = new System.IO.MemoryStream(buffer);
            stream.Position = 0;
            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(stream, Encoding.UTF8))
            {
                writer.Write(Action + "\r\n");
                foreach (string key in mProperties.Keys)
                {
                    writer.Write(string.Format("{0}: {1}\r\n", key, mProperties[key]));
                }
                writer.Write("\r\n");
            }
            return new ArraySegment<byte>(buffer, 0, (int)stream.Position);
        }
    }
}
