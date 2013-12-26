using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFileStorage
{
    /*
    byte[4] num
    byte[4] next
    byte[4] hascode
    byte[4] key length
    byte[1024] key data
    */
    class Bucket
    {
        public Bucket()
        {
            
        }

        public int Index
        {
            get;
            set;
        }

        private int? mNum = null;

        private int? mNext = null;

        private int? mHasCode = null;

        private string mKey = null;

        public int GetNum(System.IO.BinaryReader reader)
        {
            if (mNum == null)
            {
                Seek(reader.BaseStream, 0);
                mNum = reader.ReadInt32();
            }
            return mNum.Value;
        }
        public void SetNum(System.IO.BinaryWriter writer,int value)
        {
            Seek(writer.BaseStream, 0);
            writer.Write(value);
            mNum = null;
        }
        public void Seek(System.IO.Stream stream,int offset)
        {
            long value = 16 + (16 + HeaderMap.KEY_MAXLENGTH) * Index;
            stream.Position = value + offset;
        }

        public int GetNext(System.IO.BinaryReader reader)
        {
            if (mNext == null)
            {
                Seek(reader.BaseStream, 4);
                mNext = reader.ReadInt32();
            }
            return mNext.Value;
        }

        public void SetNext(System.IO.BinaryWriter writer,int value)
        {
            Seek(writer.BaseStream, 4);
            writer.Write(value);
            mNext = null;
        }

        public int GetHasCode(System.IO.BinaryReader reader)
        {
            if (mHasCode == null)
            {
                Seek(reader.BaseStream, 8);
                mHasCode = reader.ReadInt32();
            }
            return mHasCode.Value;
        }

        public void SetHasCode(System.IO.BinaryWriter writer, int value)
        {
            Seek(writer.BaseStream, 8);
            writer.Write(value);
            mHasCode = null;
        }

        public string GetKey(System.IO.BinaryReader reader)
        {
            if (mKey == null)
            {
                Seek(reader.BaseStream, 12);
                int keylenth = reader.ReadInt32();
                if (keylenth > 0)
                {
                    byte[] data = reader.ReadBytes(keylenth);
                    mKey = Encoding.UTF8.GetString(data);
                }
            }
            return mKey;
        }

        public void SetKey(System.IO.BinaryWriter writer, string value)
        {
            int keylen = 0;
            Seek(writer.BaseStream, 12);
            if (!string.IsNullOrEmpty(value))
            {
                keylen = Encoding.UTF8.GetBytes(value, 0, value.Length, KeyBuffer, 0);
                writer.Write(keylen);
                writer.Write(KeyBuffer, 0, keylen);
            }
            else
            {
                writer.Write(keylen);
            }
            mKey = null;
        }      

        [ThreadStatic]
        private static byte[] mKeyBuffer;

        public static byte[] KeyBuffer
        {
            get
            {
                if(mKeyBuffer ==null)
                    mKeyBuffer = new byte[1024];
                return mKeyBuffer;
            }
        }

      
    }
}
