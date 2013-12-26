using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace IKende.com.core
{
    public class TimeWatch
    {
       
       

        [ThreadStatic]
        private static StopWatchExtend mSWE = null;

        private static StopWatchExtend SWE
        {
            get
            {
                if (mSWE == null)
                    mSWE = new StopWatchExtend();
                return mSWE;
            }
        }

        [Conditional("DEBUG")]
        public static void Clean()
        {
            if (mSWE != null)
                mSWE.Dispose();
            mSWE = null;
        }

        [Conditional("DEBUG")]
        public static void ________________________________________________________()
        {
            SWE.End();
        }

        [Conditional("DEBUG")]
        public static void ________________________________________________________(string name)
        {
            SWE.Start(name);
        }

        [Conditional("DEBUG")]
        public static void ________________________________________________________(string format, params object[] datas)
        {
            ________________________________________________________(string.Format(format, datas));
        }

        [Conditional("DEBUG")]
        public static void Report<T, T1, T2>(Action<StopWatchExtend, T, T1, T2> handler, T obj, T1 obj1, T2 obj2)
        {
            handler(SWE, obj, obj1, obj2);
        }

        [Conditional("DEBUG")]
        public static void Report<T, T1>(Action<StopWatchExtend, T, T1> handler, T obj, T1 obj1)
        {
            handler(SWE, obj, obj1);
        }

        [Conditional("DEBUG")]
        public static void Report<T>(Action<StopWatchExtend, T> handler, T obj)
        {
            handler(SWE, obj);
        }

        [Conditional("DEBUG")]
        public static void Report(Action<StopWatchExtend> handler)
        {
            handler(SWE);
        }

        public class StopWatchExtend : IDisposable
        {
            private System.Diagnostics.Stopwatch mStopWatch;

            private WatchItem mFirstItem = null;

            private WatchItem mCurrentItem = null;

            public WatchItem Item
            {
                get
                {
                    return mFirstItem;
                }
            }





            public void End()
            {
                if (mCurrentItem != null)
                {
                    WatchItem wi = mCurrentItem;
                    wi.EndTime = mStopWatch.Elapsed.TotalMilliseconds;
                    mCurrentItem = wi.Parent;
                    wi.Parent = null;
                }
                if (mCurrentItem == null)
                {

                    mStopWatch.Stop();
                    mStopWatch = null;
                    Dispose();
                }
            }

            public void Start(string name)
            {
                if (mStopWatch == null)
                {
                    mFirstItem = new WatchItem(name, 0);
                    mStopWatch = new Stopwatch();
                    mStopWatch.Start();
                    mCurrentItem = mFirstItem;
                    mCurrentItem.StartTime = mStopWatch.Elapsed.TotalMilliseconds;
                }
                else
                {
                    WatchItem item = new WatchItem(name, 0);
                    item.StartTime = mStopWatch.Elapsed.TotalMilliseconds;
                    mCurrentItem.Childs.Add(item);
                    item.Parent = mCurrentItem;
                    mCurrentItem = item;
                }

            }

            public void Start(string format, params object[] datas)
            {
                Start(string.Format(format, datas));
            }

            private const string SPACE = "                                                                                                                                                                                                           ";

            public StopWatchExtend()
            {

            }
            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                if (mFirstItem != null)
                {
                    mFirstItem.ToString(sb, 1);
                }
                return sb.ToString();
            }
            public class WatchItem
            {
                public WatchItem(string name, double time)
                {
                    Name = name;
                    StartTime = time;
                    Childs = new List<WatchItem>(32);
                }
                public string Name;
                public double StartTime;
                public double EndTime;

                internal WatchItem Parent
                {
                    get;
                    set;
                }
                internal IList<WatchItem> Childs
                {
                    get;
                    set;
                }
                public void ToString(StringBuilder sb, int space)
                {
                    sb.AppendFormat("*{0}{1} [Use Time:{2:0.000}ms]\r\n", SPACE.Substring(0, space), Name, EndTime - StartTime);
                    foreach (WatchItem item in Childs)
                    {
                        item.ToString(sb, space + 3);
                    }
                    // sb.AppendFormat("*{0}{1} End:{2:0.000}ms|Use Time:{3:0.000}ms\r\n", SPACE.Substring(0, space), Name, EndTime, EndTime -StartTime);
                }
            }




            public void Dispose()
            {

            }
        }
       
    }
}
