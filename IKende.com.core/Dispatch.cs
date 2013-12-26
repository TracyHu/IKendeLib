using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IKende.com.core
{
    /// <summary>
    /// 作业调度器
    /// </summary>
    public class Dispatch
    {
        /// <summary>
        /// 构建作业调度器
        /// </summary>
        public Dispatch()
        {
            System.Threading.ThreadPool.QueueUserWorkItem(OnRun);
        }

        private System.Threading.Semaphore mSemaphore = new System.Threading.Semaphore(0, 1);

        private int mCounter = 0;

        private System.Collections.Concurrent.ConcurrentQueue<ITack> mQueue = new System.Collections.Concurrent.ConcurrentQueue<ITack>();

        private ITack GetCmd()
        {
            ITack result = null;
            mQueue.TryDequeue(out result);
            if (result != null)
                System.Threading.Interlocked.Decrement(ref mCounter);
            return result;
        }
        /// <summary>
        /// 添加作业
        /// </summary>
        /// <param name="cmd">作业</param>
        public void Add(ITack cmd)
        {

            if (System.Threading.Interlocked.CompareExchange(ref mCounter, 1, -1) == -1)
            {
                mSemaphore.Release();
            }
            else
            {
                System.Threading.Interlocked.Increment(ref mCounter);
            }
            mQueue.Enqueue(cmd);

        }

        private void OnRun(object state)
        {
            while (true)
            {
                ITack cmd = GetCmd();
                if (cmd == null)
                {
                    if (System.Threading.Interlocked.CompareExchange(ref mCounter, -1, 0) == 0)
                    {
                        mSemaphore.WaitOne();
                    }
                }
                else
                {
                    try
                    {
                        cmd.Execute();
                        Console.WriteLine(cmd);
                    }
                    catch
                    {
                    }
                }
            }
        }
        /// <summary>
        /// 作业描述接口
        /// </summary>
        public interface ITack
        {
            void Execute();
        }
    }
}
