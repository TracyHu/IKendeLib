using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IKende.com.SNService.Test
{
    class Program
    {

        private static IKende.com.SNService.Api.SNClient client;

        private static Dictionary<ulong, ulong> mKeys = new Dictionary<ulong, ulong>(50000000);

        private static int mCount;

        private static int mLastCount;

        static void Main(string[] args)
        {
            client = new Api.SNClient("127.0.0.1", 8088);
            System.Threading.ThreadPool.QueueUserWorkItem(Test);
            System.Threading.ThreadPool.QueueUserWorkItem(Test);
            System.Threading.ThreadPool.QueueUserWorkItem(Test);
            System.Threading.ThreadPool.QueueUserWorkItem(Test);
            System.Threading.ThreadPool.QueueUserWorkItem(Test);
            System.Threading.ThreadPool.QueueUserWorkItem(Test);
            System.Threading.ThreadPool.QueueUserWorkItem(Test);
            System.Threading.ThreadPool.QueueUserWorkItem(Test);
            System.Threading.ThreadPool.QueueUserWorkItem(Test);
            System.Threading.ThreadPool.QueueUserWorkItem(Test);
            System.Threading.ThreadPool.QueueUserWorkItem(Test);
            System.Threading.ThreadPool.QueueUserWorkItem(Test);
            while (true)
            {
                Console.WriteLine("{0}/秒|总数量:{1}",mCount - mLastCount,mCount);
                mLastCount = mCount;
                System.Threading.Thread.Sleep(1000);
            }
        }
        private static void Test(object state)
        {
            while (true)
            {
                ulong value = client.GetValue();
                System.Threading.Interlocked.Increment(ref mCount);
                mKeys.Add(value, value);
            }
        }
    }
}
