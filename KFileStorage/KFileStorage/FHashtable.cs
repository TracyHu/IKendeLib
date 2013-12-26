using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFileStorage
{
    public class FHashtable:IDisposable
    {
        public FHashtable(string path, string table, int recordsize, int size)
        {
            if (path.LastIndexOf(System.IO.Path.DirectorySeparatorChar) != path.Length - 1)
            {
                path += System.IO.Path.DirectorySeparatorChar;
            }
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            string mapfile = path + table + ".h";
            string datafile = path+ table+".data";
            Formater = new StringFormater();
            mHeaderMap = new HeaderMap(mapfile, size);
            mDataTable = new DataTable(datafile, recordsize, mHeaderMap.Capacity);
            for (int i = 0; i < 50; i++)
            {
                mBufferPool.Push(new byte[recordsize]);

            }
        }

        private byte[] Pop()
        {
            lock (mBufferPool)
            {
                return mBufferPool.Pop();
            }
        }

        private void Push(byte[] data)
        {
            lock (mBufferPool)
            {
                mBufferPool.Push(data);
            }
        }

        private Stack<byte[]> mBufferPool = new Stack<byte[]>();

        private DataTable mDataTable;

        private HeaderMap mHeaderMap;

        public IDataFormater Formater
        {
            get;
            set;
        }

        public void Clear()
        {
            mHeaderMap.Clear();
        }

        public bool Remove(string key)
        {
            return mHeaderMap.Remove(key);
        }

        public void Set(string key, object data)
        {
            int index = mHeaderMap.Set(key);
            byte[] buffer = Pop();
            try
            {
                ArraySegment<byte> db = Formater.Write(data, buffer);
                mDataTable.Set(db, index);
            }
            finally
            {
                Push(buffer);
            }

        }

        public object Get(string key)
        {
            int index = mHeaderMap.Get(key);
            if (index >= 0)
            {
                byte[] buffer = Pop();
                try
                {
                    ArraySegment<byte> db = mDataTable.Get(index, buffer);
                    return Formater.Read(db);
                }
                finally
                {
                    Push(buffer);
                }
            }
            return null;
        }

        public T Get<T>(string key)
        {
            return (T)Get(key);
        }

        public System.Collections.ICollection Keys
        {
            get
            {
                return mHeaderMap.Keys;
            }
        }


        public void Dispose()
        {
            mDataTable.Dispose();
            mHeaderMap.Dispose();
        }
    }
}
