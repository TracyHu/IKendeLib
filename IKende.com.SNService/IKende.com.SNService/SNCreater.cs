using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IKende.com.SNService
{
    public class SNCreater
    {

        private static DateTime mDefaultTime = new DateTime(2013, 1, 1);

        public SNCreater()
        {
            StartValue = 1;
            Step = 1;
            mLastSecond = Convert.ToUInt64((DateTime.Now - mDefaultTime).TotalMilliseconds);
           
            Sequence = 1;

        }

        public SNCreater(uint start, uint step)
        {
            StartValue = start;
            Step = step;
            Sequence = (ushort)start;
        }

        private ulong mLastSecond;

        private ushort Sequence;

        public ulong GetValue()
        {
            lock (this)
            {
                ulong result;
                ulong second = Convert.ToUInt64((DateTime.Now - mDefaultTime).TotalMilliseconds);
                if (second > mLastSecond)
                {
                    Sequence = (ushort)StartValue;
                    mLastSecond = second;
                }
                result = (second << 16) | (ulong)Sequence;
                Sequence += (ushort)Step;
                return result;
            }
        }

        public Byte[] GetValueData()
        {
            return BitConverter.GetBytes(GetValue());
        }

        public uint StartValue
        {
            get;
            set;
        }

        public uint Step
        {
            get;
            set;
        }
    }
}
