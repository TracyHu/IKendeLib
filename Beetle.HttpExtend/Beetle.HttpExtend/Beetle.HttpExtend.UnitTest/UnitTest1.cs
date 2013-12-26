using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
namespace Beetle.HttpExtend.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void HeaderImport()
        {
            string header = "Post\r\nname:henry\r\nemail:\r\n\r\n";
            byte[] data = Encoding.UTF8.GetBytes(header);
            int offset = 0;
            int count = data.Length;
            byte[] buffer = new byte[1024 * 4];
            HttpHeader hh = new HttpHeader(buffer);
            if (hh.Import(data, ref offset, ref count))
            {
                Assert.AreEqual(hh.RequestType, "Post");
                Assert.AreEqual(hh["name"], "henry");
                Assert.AreEqual(hh["email"], "");
            }

        }

        [TestMethod]
        public void HeaderImport1()
        {
            string header = "Post\r\nname:henry\r\nemail:henryfan@msn.com\r\n\r\n";
            byte[] data = Encoding.UTF8.GetBytes(header);
            int offset = 0;
            int count = data.Length;
            byte[] buffer = new byte[1024 * 4];
            HttpHeader hh = new HttpHeader(buffer);

            if (hh.Import(data, ref offset, ref count))
            {
                Assert.AreEqual(hh.RequestType, "Post");
                Assert.AreEqual(hh["name"], "henry");
                Assert.AreEqual(hh["email"], "henryfan@msn.com");
                hh = new HttpHeader(buffer);
            }


            header = "Get\r\nname:henry\r\n";
            data = Encoding.UTF8.GetBytes(header);
            offset = 0;
            count = data.Length;
            hh.Import(data, ref offset, ref count);


            header = "email:henryfan@msn.com";
            data = Encoding.UTF8.GetBytes(header);
            offset = 0;
            count = data.Length;
            hh.Import(data, ref offset, ref count);

            header = "\r";
            data = Encoding.UTF8.GetBytes(header);
            offset = 0;
            count = data.Length;
            hh.Import(data, ref offset, ref count);

            header = "\n\r\n";
            data = Encoding.UTF8.GetBytes(header);
            offset = 0;
            count = data.Length;

            if (hh.Import(data, ref offset, ref count))
            {
                Assert.AreEqual(hh.RequestType, "Get");
                Assert.AreEqual(hh["name"], "henry");
                Assert.AreEqual(hh["email"], "henryfan@msn.com");
            }

        }
    }
}
