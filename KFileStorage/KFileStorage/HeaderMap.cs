using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace KFileStorage
{
    /*
     byte[4] capacity
     byte[4] count
     byte[4] freeCount
     byte[4] freeList
     */
    
    class HeaderMap:IDisposable
    {

        public const int KEY_MAXLENGTH = 128;

        private int count;

        private int freeCount;

        private int freeList=-1;

        private Bucket[] buckets;

        private System.IO.Stream mStream;

        private System.IO.Stream mReaderStream;

        private System.IO.Stream mReadOnlyStream;

        private System.IO.BinaryReader mReadOnly;

        private System.IO.BinaryReader mReader;

        private System.IO.BinaryWriter mWriter;

        private IEqualityComparer<String> comparer;

        private int mCapacity;

        public HeaderMap(string filename,int capacity )
        {
            comparer = EqualityComparer<string>.Default;
            if (!System.IO.File.Exists(filename))
            {
                CreateHeaderFile(filename, capacity);
            }
            mStream = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite);
            mReaderStream = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
            mReadOnlyStream = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
            mReader = new System.IO.BinaryReader(mReaderStream);
            mWriter = new System.IO.BinaryWriter(mStream);
            mReadOnly = new System.IO.BinaryReader(mReadOnlyStream);
            mStream.Position = 0;
            mCapacity = mReader.ReadInt32();
            count = mReader.ReadInt32();
            freeCount = mReader.ReadInt32();
            freeList = mReader.ReadInt32();
            mFileName = filename;
            LoadBucket();
        }

        private string mFileName;

        private void LoadBucket()
        {
            buckets = new Bucket[mCapacity];
            for (int i = 0; i < mCapacity; i++)
            {
                buckets[i] = new Bucket();
                buckets[i].Index = i;
            }
        }

        protected void Flush()
        {
            mStream.Position = 0;
            mWriter.Write(mCapacity);
            mWriter.Write(count);
            mWriter.Write(freeCount);
            mWriter.Write(freeList);
            mWriter.Flush();
        }

        internal static class HashHelpers
        {
            internal static readonly int[] primes = new int[]
		{
			3,
			7,
			11,
			17,
			23,
			29,
			37,
			47,
			59,
			71,
			89,
			107,
			131,
			163,
			197,
			239,
			293,
			353,
			431,
			521,
			631,
			761,
			919,
			1103,
			1327,
			1597,
			1931,
			2333,
			2801,
			3371,
			4049,
			4861,
			5839,
			7013,
			8419,
			10103,
			12143,
			14591,
			17519,
			21023,
			25229,
			30293,
			36353,
			43627,
			52361,
			62851,
			75431,
			90523,
			108631,
			130363,
			156437,
			187751,
			225307,
			270371,
			324449,
			389357,
			467237,
			560689,
			672827,
			807403,
			968897,
			1162687,
			1395263,
			1674319,
			2009191,
			2411033,
			2893249,
			3471899,
			4166287,
			4999559,
			5999471,
			7199369
		};
           
            internal static bool IsPrime(int candidate)
            {
                if ((candidate & 1) != 0)
                {
                    int num = (int)Math.Sqrt((double)candidate);
                    for (int i = 3; i <= num; i += 2)
                    {
                        if (candidate % i == 0)
                        {
                            return false;
                        }
                    }
                    return true;
                }
                return candidate == 2;
            }
          
            internal static int GetPrime(int min)
            {
                if (min < 0)
                {
                    throw new FMException("prime value is zero!");
                }
                for (int i = 0; i < HashHelpers.primes.Length; i++)
                {
                    int num = HashHelpers.primes[i];
                    if (num >= min)
                    {
                        return num;
                    }
                }
                for (int j = min | 1; j < 2147483647; j += 2)
                {
                    if (HashHelpers.IsPrime(j))
                    {
                        return j;
                    }
                }
                return min;
            }
        }

        public int Capacity
        {
            get
            {
                return mCapacity;
            }
        }

        public static void CreateHeaderFile(string filename, int capacity)
        {
            int count = HashHelpers.GetPrime(capacity);
            using (System.IO.Stream stream = System.IO.File.Create(filename))
            {
                using (System.IO.BinaryWriter writer = new System.IO.BinaryWriter(stream))
                {
                    writer.Write(count);
                    writer.Write(0);
                    writer.Write(0);
                    writer.Write(-1);

                    byte[] keydata = new byte[KEY_MAXLENGTH];
                    for (int i = 0; i < count; i++)
                    {
                        writer.Write(-1);
                        writer.Write(0);
                        writer.Write(0);
                        writer.Write(0);
                        writer.Write(keydata, 0, KEY_MAXLENGTH);
                    }
                    writer.Flush();
                }
            }
        }

        public int Count
        {
            get
            {
                return count - freeCount;
            }
        }

        public int Set(string key)
        {
            lock (this)
            {
               
                if (key == null)
                {
                    throw new FMException("key is null!");
                }
                if (key.Length > HeaderMap.KEY_MAXLENGTH)
                {
                    throw new FMException("key to long!");
                }
                if (count >= mCapacity)
                {
                    throw new FMException("file maptable space overflow!");
                }
                int num = this.comparer.GetHashCode(key) & 2147483647;
                int num2 = num % this.buckets.Length;
                for (int i = this.buckets[num2].GetNum(mReader); i >= 0; i = this.buckets[i].GetNext(mReader))
                {
                    if (this.buckets[i].GetHasCode(mReader) == num && this.comparer.Equals(this.buckets[i].GetKey(mReader), key))
                    {
                        return i;
                    }
                }
                int num3;
                if (this.freeCount > 0)
                {
                    num3 = this.freeList;
                    this.freeList = this.buckets[num3].GetNext(mReader);
                    this.freeCount--;
                }
                else
                {
                    if (this.count == mCapacity)
                    {
                        Resize();
                    }
                    num3 = this.count;
                    this.count++;
                }
                this.buckets[num3].SetHasCode(mWriter, num);// .HashCode = num;
                this.buckets[num3].SetNext(mWriter, this.buckets[num2].GetNum(mReader));
                this.buckets[num3].SetKey(mWriter, key);
                this.buckets[num2].SetNum(mWriter, num3);
                Flush();
                return num3;
            }
        }
        
        private void Resize()
        {
            /*
            mStream.Position = mStream.Length;
            byte[] keydata = new byte[KEY_MAXLENGTH];
            for(int i=0;i<this.count;i++)
            {
                    mWriter .Write(-1);
                    mWriter.Write(0);
                    mWriter.Write(0);
                    mWriter.Write(0);
                    mWriter.Write(keydata, 0, KEY_MAXLENGTH);        
            }
            int prime = HashHelpers.GetPrime(this.count * 2);
            mCapacity = prime;
           int[] array = new int[prime];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = -1;
            }
            Dictionary<TKey, TValue>.Entry[] array2 = new Dictionary<TKey, TValue>.Entry[prime];
            Array.Copy(this.entries, 0, array2, 0, this.count);
            for (int j = 0; j < this.count; j++)
            {
                int num = array2[j].hashCode % prime;
                array2[j].next = array[num];
                array[num] = j;
            }
            this.buckets = array;
            this.entries = array2;
             */
        }

        public void Clear()
        {
            lock (this)
            {
                if (this.count > 0)
                {
                    for (int i = 0; i < this.buckets.Length; i++)
                    {
                        this.buckets[i].SetNum(mWriter, -1);
                        this.buckets[i].SetNext(mWriter, 0);
                        this.buckets[i].SetHasCode(mWriter, 0);
                        this.buckets[i].SetKey(mWriter, null);
                    }
                   
                    this.freeList = -1;
                    this.count = 0;
                    this.freeCount = 0;
                    Flush();
                }
            }
        }

        public bool Remove(string key)
        {
            lock (this)
            {
                if (key == null)
                {
                    throw new FMException("key is null!");
                }
                if (this.buckets != null)
                {
                    int num = this.comparer.GetHashCode(key) & 2147483647;
                    int num2 = num % this.buckets.Length;
                    int num3 = -1;
                    for (int i = this.buckets[num2].GetNum(mReader); i >= 0; i = this.buckets[i].GetNum(mReader))
                    {
                        if (this.buckets[i].GetHasCode(mReader) == num && this.comparer.Equals(this.buckets[i].GetKey(mReader), key))
                        {
                            if (num3 < 0)
                            {
                                this.buckets[num2].SetNum(mWriter, this.buckets[i].GetNext(mReader));
                            }
                            else
                            {

                                this.buckets[num3].SetNext(mWriter, this.buckets[i].GetNext(mReader));
                            }
                            this.buckets[i].SetHasCode(mWriter, -1);
                            this.buckets[i].SetNext(mWriter, this.freeList);
                            this.buckets[i].SetKey(mWriter, null);
                            this.freeList = i;
                            this.freeCount++;
                            Flush();
                            return true;
                        }
                        num3 = i;
                    }
                }
                return false;
            }
        }

        public int Get(string key)
        {
            lock (mReadOnly)
            {
                if (key == null)
                {
                    throw new FMException("key is null!");
                }
                if (this.buckets != null)
                {
                    int num = this.comparer.GetHashCode(key) & 2147483647;
                    for (int i = this.buckets[num % this.buckets.Length].GetNum(mReadOnly); i >= 0; i = this.buckets[i].GetNext(mReadOnly))
                    {
                        if (this.buckets[i].GetHasCode(mReadOnly) == num && this.comparer.Equals(this.buckets[i].GetKey(mReadOnly), key))
                        {
                            return i;
                        }
                    }
                }
                return -1;
            }
        }

        public ICollection Keys
        {
            get
            {
                return new KeyCollection(this);
            }
        }

        private string GetBucketKey(Bucket item)
        {
            lock (mReadOnly)
            {
                return item.GetKey(mReadOnly);
            }
        }

        private int GetBucketHashCode(Bucket item)
        {
            lock (mReadOnly)
            {
                return item.GetHasCode(mReadOnly);
            }
        }

        public void Dispose()
        {
            mStream.Flush();
            mStream.Dispose();
            mReaderStream.Dispose();
            mReadOnlyStream.Dispose();
        }

        public void CopyKeys(Array keys, int arrayIndex)
        {
            foreach (string key in keys)
            {
                keys.SetValue(key, arrayIndex);
                arrayIndex++;
            }
        }

        [Serializable]
        private class KeyCollection : ICollection, IEnumerable
        {
            private HeaderMap _hashtable;

            public virtual bool IsSynchronized
            {
                get
                {
                    return false;
                }
            }

            public virtual object SyncRoot
            {
                get
                {
                    return this;
                }
            }

            public virtual int Count
            {
                get
                {
                    return this._hashtable.count;
                }
            }

            internal KeyCollection(HeaderMap hashtable)
            {
                this._hashtable = hashtable;
            }

            public virtual void CopyTo(Array array, int arrayIndex)
            {
                if (array == null)
                {
                    throw new ArgumentNullException("array");
                }
                if (array.Rank != 1)
                {
                    throw new FMException("Multi arrays are not supported!");
                }
                if (arrayIndex < 0)
                {
                    throw new FMException("ArgumentOutOfRange arrayIndex must be greater than zero!");
                }
                if (array.Length - arrayIndex < this._hashtable.count)
                {
                    throw new FMException("Array plus off too small");
                }
                this._hashtable.CopyKeys(array, arrayIndex);
            }
           
            public virtual IEnumerator GetEnumerator()
            {
                return new HeaderMap.Enumerator(_hashtable);
            }
        }

        struct Enumerator : IEnumerator<string>, IDisposable, IEnumerator
        {
            private HeaderMap dictionary;

            private int index;
           
            private string currentKey;
           
            public string Current
            {
               
                get
                {
                    return this.currentKey;
                }
            }
                      
            object IEnumerator.Current
            {
               
                get
                {
                    if (this.index == 0 || this.index == this.dictionary.count + 1)
                    {
                        throw new FMException("InvalidOperation index overflow!");
                    }
                    return this.currentKey;
                }
            }
            internal Enumerator(HeaderMap dictionary)
            {
                this.dictionary = dictionary;
               
                this.index = 0;
                this.currentKey = null;
            }
         
            public void Dispose()
            {
            }
            public bool MoveNext()
            {
               
                    while (this.index < this.dictionary.count)
                    {
                        Bucket item = this.dictionary.buckets[this.index];
                        if (this.dictionary.GetBucketHashCode(item) >= 0)
                        {
                            this.currentKey = this.dictionary.GetBucketKey(item);
                            this.index++;
                            return true;
                        }
                        this.index++;
                    }
                    this.index = this.dictionary.count + 1;
                    this.currentKey = null;
                    return false;
                
            }
           
            void IEnumerator.Reset()
            {
                
                this.index = 0;
                this.currentKey = null;
            }
        }
    }
}
