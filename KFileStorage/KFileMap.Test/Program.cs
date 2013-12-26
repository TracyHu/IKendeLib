using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KFileStorage;
namespace KFileMap.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            FHashtable fht = new FHashtable("c:\\hastable", "test", 1024 * 2, 1000000);
            for (int i = 0; i < 1000; i++)
            {
                string key = GetKey(i);
                fht.Set(key, key);

            }
            for (int i = 0; i < 1000; i++)
            {
                string key = GetKey(i);
                string value = (string)fht.Get(key);
                Console.WriteLine(value);
                if (key != value)
                    throw new Exception("f");

            }
            Console.WriteLine("ok");
            Console.Read();
        }
        static string GetKey(int value)
        {
            return value.ToString("0000000");
        }
        class User
        {
            public string Name;
            public int Index;
        }
    }
}
