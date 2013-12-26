using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beetle.Tracker
{
    class Protocol
    {
        public const string COMMAND_REGISTER = "Register";

        public const string COMMAND_GETINFO = "GetInfo";

        public const string COMMAND_GET = "Get";

        public static HttpExtend.HttpHeader Register(string appName,IProperties properties)
        {
            HttpExtend.HttpHeader command = new HttpExtend.HttpHeader();
            command.Action = COMMAND_REGISTER+" " + appName;
            command.Properties = properties.ToHeaders();
            return command;
        }
        public static HttpExtend.HttpHeader GetInfo(string appName, IProperties properties)
        {
            HttpExtend.HttpHeader command = new HttpExtend.HttpHeader();
            command.Action = COMMAND_GETINFO+" " + appName;
            command.Properties = properties.ToHeaders();
            return command;
        }
        public static HttpExtend.HttpHeader Get(string appName, IProperties properties)
        {
            HttpExtend.HttpHeader command = new HttpExtend.HttpHeader();
            command.Action = COMMAND_GET + " " + appName;
            command.Properties = properties.ToHeaders();
            return command;
        }
        public static HttpExtend.HttpHeader GetResponse(IProperties properties)
        {
            HttpExtend.HttpHeader command = new HttpExtend.HttpHeader();

            command.Properties = properties.ToHeaders();
            return command;
            
        }
    }
}
