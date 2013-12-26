using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IKende.com.core
{
    public class PerformanceTesting
    {
        interface ITestItem
        {
            string Name { get; set; }
            int Count { get; set; }
            void Execute();
        }
        class Test:ITestItem
        {
            public int Count
            {
                get;
                set;
            }

            public string Name
            {
                get;
                set;
            }
            public Action Method1
            {
                get;
                set;
            }
            public void Execute()
            {
                for (int i = 0; i < Count; i++)
                {
                    OnExecute();
                }
            }
            protected virtual void OnExecute()
            {
                Method1();
            }
        }
        class Test<T> : Test
        {
            public Action<T> Method
            {
                get;
                set;
            }
            public T D1
            {
                get;
                set;
            }

            protected override void OnExecute()
            {
                Method(D1);
            }
        }
        class Test<T, T1> : Test
        {
            public Action<T,T1> Method
            {
                get;
                set;
            }
            public T1 D2
            {
                get;
                set;
            }
            public T D1
            {
                get;
                set;
            }

            protected override void OnExecute()
            {
                Method(D1,D2);
            }
        }
        class Test<T, T1, T2> : Test
        {
            public Action<T, T1,T2> Method
            {
                get;
                set;
            }
            public T2 D3
            {
                get;
                set;
            }
            public T1 D2
            {
                get;
                set;
            }
            public T D1
            {
                get;
                set;
            }

            protected override void OnExecute()
            {
                Method(D1,D2,D3);
            }
        }
        class Test<T, T1, T2, T3> : Test
        {
            public Action<T, T1, T2,T3> Method
            {
                get;
                set;
            }
            public T3 D4
            {
                get;
                set;
            }
            public T2 D3
            {
                get;
                set;
            }
            public T1 D2
            {
                get;
                set;
            }
            public T D1
            {
                get;
                set;
            }
            protected override void OnExecute()
            {
                Method(D1, D2, D3,D4);
            }
        }
        class Test<T, T1, T2, T3, T4> : Test
        {
            public Action<T, T1, T2, T3,T4> Method
            {
                get;
                set;
            }
            public T4 D5
            {
                get;
                set;
            }
            public T3 D4
            {
                get;
                set;
            }
            public T2 D3
            {
                get;
                set;
            }
            public T1 D2
            {
                get;
                set;
            }
            public T D1
            {
                get;
                set;
            }
            protected override void OnExecute()
            {
                Method(D1, D2, D3, D4,D5);
            }
        }

        public class Report
        {
            public string Name
            {
                get;
                set;
            }
            public Exception Error
            {
                get;
                set;
            }
            public double UseTime
            {
                get;
                set;
            }
            public int Count
            {
                get;
                set;
            }
            public string Detail
            {
                get
                {
                    return string.Format("{0} {1} runs use time:{2}/ms [error:{3}]", Name, Count, UseTime,Error !=null?Error.Message:"");
                }
            }
            public override string ToString()
            {
                return Detail;
            }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Report item in mReports)
            {
                sb.AppendFormat("{0}\r\n", item.ToString());
            }
            return sb.ToString();
        }
        private IList<ITestItem> mItems = new List<ITestItem>();

        private List<Report> mReports = new List<Report>();

        public List<Report> Reports
        {
            get
            {
                return mReports;
            }
        }

        public void Execute()
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            foreach (ITestItem item in mItems)
            {
                Report report = new Report();
                report.Name = item.Name;
                report.Count = item.Count;
                sw.Reset();
                sw.Start();
               
                try
                {
                    item.Execute();
                }
                catch(Exception e_)
                {
                    report.Error = e_;
                }
                sw.Stop();
                report.UseTime = sw.Elapsed.TotalMilliseconds;
                mReports.Add(report);
            }
        }

        public void Add(string name,int count, Action method)
        {
            mItems.Add(new Test() { Count=count, Name= name, Method1 = method });
        }

        public void Add<T>(string name,int count, Action<T> method, T d)
        {
            mItems.Add(new Test<T>() { D1=d, Count=count, Method= method, Name=name });
        }

        public void Add<T,T1>(string name, int count, Action<T,T1> method, T d,T1 d1)
        {
            mItems.Add(new Test<T,T1>() { D1 = d, D2=d1, Count = count, Method = method, Name = name });
        }

        public void Add<T, T1,T2>(string name, int count, Action<T, T1,T2> method, T d, T1 d1,T2 d2)
        {
            mItems.Add(new Test<T, T1,T2>() { D1 = d, D2 = d1, D3=d2 ,Count = count, Method = method, Name = name });
        }

        public void Add<T, T1, T2,T3>(string name, int count, Action<T, T1, T2,T3> method, T d, T1 d1, T2 d2,T3 d3)
        {
            mItems.Add(new Test<T, T1, T2,T3>() { D1 = d, D2 = d1, D3 = d2, D4=d3, Count = count, Method = method, Name = name });
        }

        public void Add<T, T1, T2, T3,T4>(string name, int count, Action<T, T1, T2, T3,T4> method, T d, T1 d1, T2 d2, T3 d3,T4 d4)
        {
            mItems.Add(new Test<T, T1, T2, T3,T4>() { D1 = d, D2 = d1, D3 = d2, D4 = d3,D5=d4, Count = count, Method = method, Name = name });
        }
    }
}
